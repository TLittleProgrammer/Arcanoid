using App.Scripts.External.GameStateMachine;
using App.Scripts.External.SceneManagment;
using App.Scripts.General.LoadingScreen;

namespace App.Scripts.Scenes.Bootstrap.States
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

        public async void Enter(string sceneName)
        {
            await _loadingScreen.Show(true);
            await _sceneManagementService.LoadSceneAsync(sceneName);
            await _loadingScreen.Hide();
        }

        public void Exit()
        {
            
        }
    }
}