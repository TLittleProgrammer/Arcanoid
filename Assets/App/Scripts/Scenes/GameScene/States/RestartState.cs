using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.LoadingScreen;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Animations;
using App.Scripts.Scenes.GameScene.States.Gameloop;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States
{
    public class RestartState : IState
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly List<ICurrentLevelRestartable> _currentLevelRestartables;
        private readonly IStateMachine _gameStateMachine;
        private readonly ITweenersLocator _tweenersLocator;
        private readonly IShowLevelAnimation _showLevelAnimation;
        private readonly IBallsService _ballsService;
        private readonly List<IGeneralRestartable> _generalRestartables;

        public RestartState(
            ILoadingScreen loadingScreen,
            List<ICurrentLevelRestartable> currentLevelRestartables,
            IStateMachine gameStateMachine,
            ITweenersLocator tweenersLocator,
            IShowLevelAnimation showLevelAnimation,
            IBallsService ballsService,
            List<IGeneralRestartable> generalRestartables
        )
        {
            _loadingScreen = loadingScreen;
            _currentLevelRestartables = currentLevelRestartables;
            _gameStateMachine = gameStateMachine;
            _tweenersLocator = tweenersLocator;
            _showLevelAnimation = showLevelAnimation;
            _ballsService = ballsService;
            _generalRestartables = generalRestartables;
        }
        
        public async UniTask Enter()
        {
            await _loadingScreen.Show(false);

            _tweenersLocator.RemoveAll();
            
            foreach (IGeneralRestartable generalRestartable in _generalRestartables)
            {
                generalRestartable.Restart();
            }

            foreach (ICurrentLevelRestartable restartable in _currentLevelRestartables)
            {
                restartable.Restart();
            }

            _ballsService.DespawnAll();

            await _loadingScreen.Hide();
            await _showLevelAnimation.Show();
            
            _gameStateMachine.Enter<GameLoopState>().Forget();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}