using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Command;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.MVVM.Popups.Win
{
    public class WinViewModel
    {
        private readonly WinContinueButtonAnimationSettings _winContinueButtonAnimationSettings;
        private readonly ITweenersLocator _tweenersLocator;
        private readonly CircleWinEffectSettings _circleWinEffectSettings;
        private readonly IContinueCommand _continueCommand;
        private readonly IDisableButtonsCommand _disableButtonsCommand;

        private IWinPopupView _winPopupView;
        
        public WinViewModel(
            WinContinueButtonAnimationSettings winContinueButtonAnimationSettings,
            ITweenersLocator tweenersLocator,
            CircleWinEffectSettings circleWinEffectSettings,
            IContinueCommand continueCommand,
            IDisableButtonsCommand disableButtonsCommand)
        {
            _winContinueButtonAnimationSettings = winContinueButtonAnimationSettings;
            _tweenersLocator = tweenersLocator;
            _circleWinEffectSettings = circleWinEffectSettings;
            _continueCommand = continueCommand;
            _disableButtonsCommand = disableButtonsCommand;
        }

        public void InstallView(IWinPopupView winPopupView)
        {
            _winPopupView = winPopupView;

            SubscribeButtons();
            AnimateShow();
            AnimateCircleEffect();
        }

        private void SubscribeButtons()
        {
            _winPopupView.ContinueButton.onClick.AddListener(Continue);
        }

        private void Continue()
        {
            DisableButtons();
            _continueCommand.Execute();
        }

        private void AnimateCircleEffect()
        {
            _tweenersLocator.AddTweener(_winPopupView.CircleEffect
                .DORotate(new Vector3(0f, 0f, -360f), _circleWinEffectSettings.Duration, RotateMode.LocalAxisAdd)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear));
            
            _tweenersLocator.AddTweener(_winPopupView.CircleEffect
                .DOScale(Vector3.one * 1.25f, _circleWinEffectSettings.Duration / 2f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear));
        }

        private async void AnimateShow()
        {
            _winPopupView.Transform.DOScale(Vector3.one, 1f).From(Vector3.zero).SetEase(Ease.Linear).ToUniTask().Forget();

            await _winPopupView.Transform.DORotate(new Vector3(0, 360f * 5f * GetDirection(), 0f), 1f,
                    RotateMode.FastBeyond360)
                .SetEase(Ease.Linear);
            
            _winPopupView.TextFalling.DOMoveY(_winPopupView.TargetPositionForTextFalling.transform.position.y, 0.5f).SetEase(Ease.OutBounce).ToUniTask().Forget();
            
            Sequence sequence = DOTween.Sequence();

            sequence.Append(
                    _winPopupView.ContinueButton
                        .transform
                        .DOScale(_winContinueButtonAnimationSettings.TargetScale,
                            _winContinueButtonAnimationSettings.Duration)
                        .SetDelay(_winContinueButtonAnimationSettings.Delay)
                        .SetLoops(2, LoopType.Yoyo))
                .SetEase(_winContinueButtonAnimationSettings.Ease)
                .SetLoops(-1, LoopType.Restart)
                .ToUniTask().Forget();


            _tweenersLocator.AddSequence(sequence);
        }

        private void DisableButtons()
        {
            List<Button> buttons = new()
            {
                _winPopupView.ContinueButton
            };
            
            _disableButtonsCommand.Execute(buttons);
        }

        private float GetDirection()
        {
            return Random.Range(0, 2) == 0 ? 1f : -1f;
        }
    }
}