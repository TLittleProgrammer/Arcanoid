using System.Collections.Generic;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Levels;
using App.Scripts.General.Providers;
using App.Scripts.Scenes.GameScene.Command;
using App.Scripts.Scenes.GameScene.Command.Win;
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
        private readonly ILoadNextLevelCommand _loadNextLevelCommand;
        private readonly IDisableButtonsCommand _disableButtonsCommand;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly SpriteProvider _spriteProvider;

        private IWinPopupView _winPopupView;
        
        public WinViewModel(
            WinContinueButtonAnimationSettings winContinueButtonAnimationSettings,
            ITweenersLocator tweenersLocator,
            CircleWinEffectSettings circleWinEffectSettings,
            ILoadNextLevelCommand loadNextLevelCommand,
            IDisableButtonsCommand disableButtonsCommand,
            ILevelPackInfoService levelPackInfoService,
            SpriteProvider spriteProvider)
        {
            _winContinueButtonAnimationSettings = winContinueButtonAnimationSettings;
            _tweenersLocator = tweenersLocator;
            _circleWinEffectSettings = circleWinEffectSettings;
            _loadNextLevelCommand = loadNextLevelCommand;
            _disableButtonsCommand = disableButtonsCommand;
            _levelPackInfoService = levelPackInfoService;
            _spriteProvider = spriteProvider;
        }

        public void InstallView(IWinPopupView winPopupView)
        {
            _winPopupView = winPopupView;

            SubscribeButtons();
            AnimateShow();
            AnimateCircleEffect();
            UpdateVisual();
            AnimateIfNeed();
        }

        private void UpdateVisual()
        {
            LevelPack currentPack = _levelPackInfoService.GetData().LevelPack;
            _winPopupView.BottomGalacticIcon.sprite = _spriteProvider.Sprites[currentPack.GalacticIconKey];
            _winPopupView.GalacticName.SetToken(currentPack.LocaleKey);
            _winPopupView.PassedLevelsText.text = $"{_levelPackInfoService.GetData().LevelIndex + 1}/{currentPack.Levels.Count}";
        }

        private void SubscribeButtons()
        {
            _winPopupView.ContinueButton.onClick.AddListener(Continue);
        }

        private void Continue()
        {
            DisableButtons();
            _loadNextLevelCommand.Execute();
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

        private async void AnimateIfNeed()
        {
            if (_levelPackInfoService.NeedLoadNextPack())
            {
                LevelPack nextPack = _levelPackInfoService.GetDataForNextPack();
                LevelPack currentLevelPack = _levelPackInfoService.GetData().LevelPack;

                _winPopupView.TopGalacticIcon.color = Color.clear;
                _winPopupView.TopGalacticIcon.sprite = _spriteProvider.Sprites[nextPack.GalacticIconKey];
                _winPopupView.ContinueButton.interactable = false;

                await UniTask.Delay(2000);
                await DOVirtual.Float(1f, 0f, 1f, HideObjects);

                _winPopupView.GalacticName.SetToken(nextPack.LocaleKey);

                DOVirtual.Float(0f, 1f, 1f, ShowObjects).ToUniTask().Forget();
                await DOVirtual.Float(0f, 1f, 1f, (value) =>
                {
                    int currentLevel = (int)Mathf.Lerp(currentLevelPack.Levels.Count, 0, value);
                    int maxLevelsCount = (int)Mathf.Lerp(currentLevelPack.Levels.Count, nextPack.Levels.Count, value);

                    _winPopupView.PassedLevelsText.text = $"{currentLevel.ToString()}/{maxLevelsCount.ToString()}";
                });

                _winPopupView.ContinueButton.interactable = true;
            }

        }

        private void ShowObjects(float value)
        {
            _winPopupView.GalacticName.Text.color = new Color(1f, 1f, 1f, value);
            _winPopupView.TopGalacticIcon.color = new Color(1f, 1f, 1f, value);
        }

        private void HideObjects(float value)
        {
            _winPopupView.GalacticName.Text.color = new Color(1f, 1f, 1f, value);
            _winPopupView.BottomGalacticIcon.color = new Color(1f, 1f, 1f, value);
        }
    }
}