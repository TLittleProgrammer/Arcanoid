using System.Collections.Generic;
using System.Threading.Tasks;
using App.Scripts.General.Command;
using App.Scripts.General.Levels;
using App.Scripts.General.Levels.LevelPackInfoService;
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
            LevelPack currentPack = _levelPackInfoService.LevelPackTransferData.LevelPack;
            _winPopupView.BottomGalacticIcon.sprite = _spriteProvider.Sprites[currentPack.GalacticIconKey];
            _winPopupView.GalacticName.SetToken(currentPack.LocaleKey);
            _winPopupView.PassedLevelsText.text = $"{_levelPackInfoService.LevelPackTransferData.LevelIndex + 1}/{currentPack.Levels.Count}";
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
                .DORotate(new Vector3(0f, 0f, -360f), _circleWinEffectSettings.Duration, RotateMode.WorldAxisAdd)
                .SetRelative()
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear));
            
            _tweenersLocator.AddTweener(_winPopupView.CircleEffect
                .DOScale(Vector3.one * 1.25f, _circleWinEffectSettings.Duration / 4f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear));
        }

        private async void AnimateShow()
        {
            _winPopupView.EnergyView.transform.localScale = Vector3.zero;
            _winPopupView.ContinueButton.transform.localScale = Vector3.zero;
            _winPopupView.PackViewTransform.transform.localScale = Vector3.zero;
            
            await _winPopupView.Transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBounce).ToUniTask();
            await _winPopupView.EnergyView.transform.DOScale(Vector3.one, 0.75f).From(Vector3.zero).SetEase(Ease.OutBounce).ToUniTask();
            await _winPopupView.PackViewTransform.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBounce).ToUniTask();
            
            _winPopupView.TextFalling.DOMoveY(_winPopupView.TargetPositionForTextFalling.transform.position.y, 0.5f).SetEase(Ease.OutBounce).ToUniTask().Forget();
            
            await _winPopupView.ContinueButton.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBounce).ToUniTask();
            
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

                if (nextPack is not null)
                {
                    LevelPack currentLevelPack = _levelPackInfoService.LevelPackTransferData.LevelPack;

                    _winPopupView.TopGalacticIcon.sprite = _spriteProvider.Sprites[nextPack.GalacticIconKey];
                    _winPopupView.ContinueButton.interactable = false;
                    
                    await UniTask.Delay(2000);

                    UpdateIconPositions();
                    
                    await UpdatePackNaming(nextPack, currentLevelPack);

                    _winPopupView.ContinueButton.interactable = true;
                }
            }

        }

        private void UpdateIconPositions()
        {
            Vector2 firstAnchoredPosition = _winPopupView.TopGalacticIcon.rectTransform.anchoredPosition;
            Vector2 secondAnchoredPosition = _winPopupView.BottomGalacticIcon.rectTransform.anchoredPosition;
            
            DOVirtual.Float(0f, 1f, 0.75f, value =>
            {
                _winPopupView.TopGalacticIcon.rectTransform.anchoredPosition =
                    Vector2.Lerp(firstAnchoredPosition, new Vector2(-firstAnchoredPosition.x, firstAnchoredPosition.y), value);
                
                _winPopupView.BottomGalacticIcon.rectTransform.anchoredPosition =
                    Vector2.Lerp(secondAnchoredPosition, new Vector2(-secondAnchoredPosition.x, secondAnchoredPosition.y), value);
            });
        }

        private async UniTask UpdatePackNaming(LevelPack nextPack, LevelPack currentLevelPack)
        {
            DOVirtual.Float(1f, 0f, 1f, HideObjectsText).ToUniTask().Forget();

            _winPopupView.GalacticName.SetToken(nextPack.LocaleKey);

            DOVirtual.Float(0f, 1f, 1f, ShowObjectsText).ToUniTask().Forget();

            await ChangeLevelText(currentLevelPack, nextPack);
        }

        private async UniTask ChangeLevelText(LevelPack currentLevelPack, LevelPack nextPack)
        {
            await DOVirtual.Float(0f, 1f, 1f, (value) =>
            {
                int currentLevel = (int)Mathf.Lerp(currentLevelPack.Levels.Count, 0, value);
                int maxLevelsCount = (int)Mathf.Lerp(currentLevelPack.Levels.Count, nextPack.Levels.Count, value);

                _winPopupView.PassedLevelsText.text = $"{currentLevel.ToString()}/{maxLevelsCount.ToString()}";
            });
        }

        private void ShowObjectsText(float value)
        {
            _winPopupView.GalacticName.Text.color = new Color(1f, 1f, 1f, value);
        }

        private void HideObjectsText(float value)
        {
            _winPopupView.GalacticName.Text.color = new Color(1f, 1f, 1f, value);
        }
    }
}