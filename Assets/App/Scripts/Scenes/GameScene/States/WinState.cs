﻿using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Constants;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Levels;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Popups;
using App.Scripts.Scenes.GameScene.Time;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.States
{
    public class WinState : IState
    {
        private readonly ITimeScaleAnimator _timeScaleAnimator;
        private readonly IPopupService _popupService;
        private readonly RootUIViewProvider _rootUIViewProvider;
        private readonly IStateMachine _gameStateMachine;
        private readonly ILevelPackInfoService _levelPackInfoService;

        private WinPopupView _winPopupView;

        public WinState(
            ITimeScaleAnimator timeScaleAnimator,
            IPopupService popupService,
            RootUIViewProvider rootUIViewProvider,
            IStateMachine gameStateMachine,
            ILevelPackInfoService levelPackInfoService)
        {
            _popupService = popupService;
            _timeScaleAnimator = timeScaleAnimator;
            _popupService = popupService;
            _rootUIViewProvider = rootUIViewProvider;
            _gameStateMachine = gameStateMachine;
            _levelPackInfoService = levelPackInfoService;
        }
        
        public async UniTask Enter()
        {
            await _timeScaleAnimator.Animate(0f);
            
            ShowPopup();
            UpdateVisual();
            Subscribe();
            AnimateIfNeed();
        }

        public async UniTask Exit()
        {
            _winPopupView.ContinueButton.onClick.RemoveListener(OnContinueClicked);
            
            await UniTask.CompletedTask;
        }

        private async void AnimateIfNeed()
        {
            if (_levelPackInfoService.NeedLoadNextPack())
            {
                LevelPack nextPack = _levelPackInfoService.GetDataForNextPack();
                LevelPack currentLevelPack = _levelPackInfoService.GetData().LevelPack;
                
                _winPopupView.TopGalacticIcon.color = Color.clear;
                _winPopupView.TopGalacticIcon.sprite = nextPack.GalacticIcon;
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

        private void UpdateVisual()
        {
            LevelPack currentPack = _levelPackInfoService.GetData().LevelPack;
            _winPopupView.BottomGalacticIcon.sprite = currentPack.GalacticIcon;
            _winPopupView.GalacticName.SetToken(currentPack.LocaleKey);
            _winPopupView.PassedLevelsText.text = $"{_levelPackInfoService.GetData().LevelIndex + 1}/{currentPack.Levels.Count}";
        }

        private void Subscribe()
        {
            _winPopupView.ContinueButton.onClick.AddListener(OnContinueClicked);
        }

        private void ShowPopup()
        {
            _winPopupView = (WinPopupView)_popupService.Show<WinPopupView>(_rootUIViewProvider.PopupUpViewProvider);
        }

        private void OnContinueClicked()
        {
            _winPopupView.ContinueButton.interactable = false;

            if (_levelPackInfoService.NeedLoadNextPackOrLevel())
            {
                _gameStateMachine.Enter<LoadNextLevelState>();
            }
            else
            {
                _gameStateMachine.Enter<LoadSceneFromMainMenuState, string>(SceneNaming.MainMenu);
            }
        }
    }
}