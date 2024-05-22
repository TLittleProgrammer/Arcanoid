﻿using App.Scripts.External.GameStateMachine;
using App.Scripts.General.InfoBetweenScenes;
using App.Scripts.General.Popup;
using App.Scripts.General.States;
using App.Scripts.Scenes.GameScene.Dotween;
using App.Scripts.Scenes.GameScene.Popups;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States
{
    public class LoadSceneFromMainMenuState : IState<string>
    {
        private readonly IStateMachine _projectStateMachine;
        private readonly IPopupService _popupService;
        private readonly ITweenersLocator _tweenersLocator;
        private readonly InfoBetweenScenes _infoBetweenScenes;

        public LoadSceneFromMainMenuState(
            IStateMachine projectStateMachine,
            IPopupService popupService,
            ITweenersLocator tweenersLocator,
            InfoBetweenScenes infoBetweenScenes)
        {
            _projectStateMachine = projectStateMachine;
            _popupService = popupService;
            _tweenersLocator = tweenersLocator;
            _infoBetweenScenes = infoBetweenScenes;
        }
        
        public async UniTask Enter(string sceneName)
        {
            _infoBetweenScenes.NeedShowLevelPackContainer = true;
            _projectStateMachine.Enter<LoadingSceneState, string, bool>(sceneName, false);
            
            _tweenersLocator.RemoveAll();
            await _popupService.CloseAll();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}