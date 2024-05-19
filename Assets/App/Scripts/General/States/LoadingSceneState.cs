﻿using App.Scripts.External.GameStateMachine;
using App.Scripts.External.SceneManagment;
using App.Scripts.General.LoadingScreen;

namespace App.Scripts.General.States
{
    public class LoadingSceneState : IState<string, bool>
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly ISceneManagementService _sceneManagementService;

        public LoadingSceneState(ILoadingScreen loadingScreen,
            ISceneManagementService sceneManagementService)
        {
            _loadingScreen = loadingScreen;
            _sceneManagementService = sceneManagementService;
        }

        public async void Enter(string sceneName, bool showQuickly)
        {
            await _loadingScreen.Show(showQuickly);
            await _sceneManagementService.LoadSceneAsync(sceneName);
            await _loadingScreen.Hide();
        }

        public void Exit()
        {
        }
    }
}