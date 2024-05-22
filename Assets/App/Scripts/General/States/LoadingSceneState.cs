using System.Threading.Tasks;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.SceneManagment;
using App.Scripts.General.LoadingScreen;
using Cysharp.Threading.Tasks;

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

        public async UniTask Enter(string sceneName, bool showQuickly)
        {
            await _loadingScreen.Show(showQuickly);
            await _sceneManagementService.LoadSceneAsync(sceneName);
            await _loadingScreen.Hide();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}