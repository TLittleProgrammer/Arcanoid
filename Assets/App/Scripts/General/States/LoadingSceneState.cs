using System;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.SceneManagment;
using App.Scripts.General.LoadingScreen;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.States
{
    public class LoadingSceneState : IState<string, bool, Action>
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly ISceneManagementService _sceneManagementService;

        public LoadingSceneState(ILoadingScreen loadingScreen,
            ISceneManagementService sceneManagementService)
        {
            _loadingScreen = loadingScreen;
            _sceneManagementService = sceneManagementService;
        }

        public async UniTask Enter(string sceneName, bool showQuickly, Action sceneLoaded = null)
        {
            await _loadingScreen.Show(showQuickly);
            await _sceneManagementService.LoadSceneAsync(sceneName);

            if (sceneLoaded is not null)
            {
                sceneLoaded.Invoke();
            }
            
            await _loadingScreen.Hide();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}