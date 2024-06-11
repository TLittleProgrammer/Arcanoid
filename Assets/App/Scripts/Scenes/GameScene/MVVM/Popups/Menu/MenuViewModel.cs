using System.Collections.Generic;
using App.Scripts.General.Command;
using App.Scripts.Scenes.GameScene.Command;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Popups;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.MVVM.Popups.Menu
{
    public sealed class MenuViewModel
    {
        private readonly IContinueCommand _continueCommand;
        private readonly IBackCommand _backCommand;
        private readonly IRestartCommand _restartCommand;
        private readonly ISkipLevelCommand _skipLevelCommand;
        private readonly IDisableButtonsCommand _disableButtonsCommand;
        private readonly ITweenersLocator _tweenersLocator;

        private IMenuPopupView _menuPopupView;
        
        public MenuViewModel(
            IContinueCommand continueCommand,
            IBackCommand backCommand,
            IRestartCommand restartCommand,
            ISkipLevelCommand skipLevelCommand,
            IDisableButtonsCommand disableButtonsCommand,
            ITweenersLocator tweenersLocator)
        {
            _continueCommand = continueCommand;
            _backCommand = backCommand;
            _restartCommand = restartCommand;
            _skipLevelCommand = skipLevelCommand;
            _disableButtonsCommand = disableButtonsCommand;
            _tweenersLocator = tweenersLocator;
        }

        public void InitializeView(IMenuPopupView menuPopupView)
        {
            _menuPopupView = menuPopupView;
            SubscribeOnCommands(menuPopupView);
            Show();
        }

        private void Show()
        {
            Sequence sequence = DOTween.Sequence(); 
            sequence
                .Append(_menuPopupView.Transform.DOScale(Vector3.one, 1f).From(Vector3.zero))
                .Append(_menuPopupView.RestartButton.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce))
                .Append(_menuPopupView.BackButton.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce))
                .Append(_menuPopupView.ContinueButton.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce))
                .Append(_menuPopupView.SkipLevelButton.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce));
        }

        private void SubscribeOnCommands(IMenuPopupView menuPopupView)
        {
            menuPopupView.ContinueButton.onClick.AddListener(Continue);
            menuPopupView.RestartButton.onClick.AddListener(Restart);
            menuPopupView.BackButton.onClick.AddListener(Back);
            menuPopupView.SkipLevelButton.onClick.AddListener(SkipLevel);
        }

        private void Restart()
        {
            DisableButtons();
            _restartCommand.Execute();
        }

        private async void Continue()
        {
            DisableButtons();

            await CloseView();
            
            _continueCommand.Execute();
        }

        private void Back()
        {
            DisableButtons();

            _backCommand.Execute();
        }

        private void SkipLevel()
        {
            DisableButtons();
            _skipLevelCommand.Execute();
        }

        private UniTask CloseView()
        {
            Sequence sequence = DOTween.Sequence();

            sequence
                .Append(AnimateScale(_menuPopupView.ContinueButton.transform, Vector3.zero, 0.25f, Ease.OutBounce))
                .Append(AnimateScale(_menuPopupView.BackButton.transform, Vector3.zero, 0.25f, Ease.OutBounce))
                .Append(AnimateScale(_menuPopupView.RestartButton.transform, Vector3.zero, 0.25f, Ease.OutBounce))
                .Append(AnimateScale(_menuPopupView.Transform, Vector3.zero, 0.75f, Ease.InOutBounce));
            
            _tweenersLocator.AddSequence(sequence);

            return sequence.ToUniTask();
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> AnimateScale(Transform transform, Vector3 targetScale, float duration, Ease ease)
        {
            return transform.DOScale(targetScale, duration).SetEase(ease);
        }

        private void DisableButtons()
        {
            List<Button> buttons = new List<Button>()
            {
                _menuPopupView.BackButton,
                _menuPopupView.ContinueButton,
                _menuPopupView.RestartButton,
            };
            
            _disableButtonsCommand.Execute(buttons);
        }
    }
}