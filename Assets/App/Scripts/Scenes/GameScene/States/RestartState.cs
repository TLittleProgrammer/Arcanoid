using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.LoadingScreen;
using App.Scripts.Scenes.GameScene.Infrastructure;

namespace App.Scripts.Scenes.GameScene.States
{
    public class RestartState : IState
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly IEnumerable<IRestartable> _restartables;
        private readonly IStateMachine _gameStateMachine;

        public RestartState(
            ILoadingScreen loadingScreen,
            IEnumerable<IRestartable> restartables,
            IStateMachine gameStateMachine
            )
        {
            _loadingScreen = loadingScreen;
            _restartables = restartables;
            _gameStateMachine = gameStateMachine;
        }
        
        public async void Enter()
        {
            await _loadingScreen.Show(false);
            
            foreach (IRestartable restartable in _restartables)
            {
                restartable.Restart();
            }
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            _loadingScreen.Hide();
        }
    }
}