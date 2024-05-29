using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.LoadingScreen;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Animations;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States
{
    public class RestartState : IState
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly List<IRestartable> _restartables;
        private readonly IStateMachine _gameStateMachine;
        private readonly ITweenersLocator _tweenersLocator;
        private readonly IShowLevelAnimation _showLevelAnimation;
        private readonly IBallsService _ballsService;

        public RestartState(
            ILoadingScreen loadingScreen,
            List<IRestartable> restartables,
            IStateMachine gameStateMachine,
            ITweenersLocator tweenersLocator,
            IShowLevelAnimation showLevelAnimation,
            IBallsService ballsService
        )
        {
            _loadingScreen = loadingScreen;
            _restartables = restartables;
            _gameStateMachine = gameStateMachine;
            _tweenersLocator = tweenersLocator;
            _showLevelAnimation = showLevelAnimation;
            _ballsService = ballsService;
        }
        
        public async UniTask Enter()
        {
            await _loadingScreen.Show(false);

            _tweenersLocator.RemoveAll();
            
            foreach (IRestartable restartable in _restartables)
            {
                restartable.Restart();
            }
            
            _ballsService.DespawnAll();

            await _loadingScreen.Hide();
            await _showLevelAnimation.Show();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}