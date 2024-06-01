using App.Scripts.External.GameStateMachine;
using App.Scripts.External.SceneManagment;
using App.Scripts.General.Constants;
using App.Scripts.General.LoadingScreen;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.States
{
    public class LoadingSceneState : IState<string>
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly ISceneManagementService _sceneManagementService;

        public LoadingSceneState(ILoadingScreen loadingScreen,
            ISceneManagementService sceneManagementService)
        {
            _loadingScreen = loadingScreen;
            _sceneManagementService = sceneManagementService;
        }

        public async UniTask Enter(string sceneName)
        {
            await _loadingScreen.Show(false);
            await _sceneManagementService.LoadSceneAsync(sceneName);
            _loadingScreen.Hide().Forget();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}