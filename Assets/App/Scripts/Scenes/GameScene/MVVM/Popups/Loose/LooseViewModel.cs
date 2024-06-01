using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Command;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Popups;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.MVVM.Popups.Loose
{
    public class LooseViewModel
    {
        private readonly IDisableButtonsCommand _disableButtonsCommand;
        private readonly IRestartCommand _restartCommand;
        private readonly ITweenersLocator _tweenersLocator;

        private ILoosePopupView _loosePopupView;
        
        public LooseViewModel(
            IDisableButtonsCommand disableButtonsCommand,
            IRestartCommand restartCommand,
            ITweenersLocator tweenersLocator)
        {
            _disableButtonsCommand = disableButtonsCommand;
            _restartCommand = restartCommand;
            _tweenersLocator = tweenersLocator;
        }

        public void InstallView(ILoosePopupView loosePopupView)
        {
            _loosePopupView = loosePopupView;
            
            SubscribeOnButtonClicks();
            AnimateView();
        }

        private void AnimateView()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(
                    _loosePopupView.RestartButton
                        .transform
                        .DOScale(1f, 0.25f)
                        .SetDelay(1f))
                .SetEase(Ease.InOutBounce);
            
            _tweenersLocator.AddSequence(sequence);
        }

        private void SubscribeOnButtonClicks()
        {
            _loosePopupView.RestartButton.onClick.AddListener(Restart);
        }

        private void Restart()
        {
            DisableButtons();
            
            _restartCommand.Execute();
        }

        private void DisableButtons()
        {
            List<Button> buttons = new()
            {
                _loosePopupView.RestartButton
            };
            
            _disableButtonsCommand.Execute(buttons);
        }
    }
}