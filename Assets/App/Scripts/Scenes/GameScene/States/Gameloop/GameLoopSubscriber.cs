using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress;

namespace App.Scripts.Scenes.GameScene.States.Gameloop
{
    public class GameLoopSubscriber
    {
        private readonly IBallsService _ballsService;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IStateMachine _stateMachine;
        private readonly IHealthContainer _healthContainer;

        public GameLoopSubscriber(
            IBallsService ballsService,
            ILevelProgressService levelProgressService,
            IStateMachine stateMachine,
            IHealthContainer healthContainer)
        {
            _ballsService = ballsService;
            _levelProgressService = levelProgressService;
            _stateMachine = stateMachine;
            _healthContainer = healthContainer;
        }
        
        public void SubscribeAll()
        {
            _levelProgressService.ProgressChanged += _ballsService.UpdateSpeedByProgress;
            _levelProgressService.LevelPassed += OnLevelPassed;
            _healthContainer.LivesAreWasted   += OnLivesAreWasted;
        }

        public void DescribeAll()
        {
            _levelProgressService.ProgressChanged -= _ballsService.UpdateSpeedByProgress;
            _levelProgressService.LevelPassed -= OnLevelPassed;
            _healthContainer.LivesAreWasted   -= OnLivesAreWasted;
        }
        
        private void OnLivesAreWasted()
        {
            _stateMachine.Enter<LooseState>();
        }

        private void OnLevelPassed()
        {
            _stateMachine.Enter<WinState>();
        }
    }
}