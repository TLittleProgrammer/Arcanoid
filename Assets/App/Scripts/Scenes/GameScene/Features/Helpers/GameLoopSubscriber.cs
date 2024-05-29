using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.LevelProgress;

namespace App.Scripts.Scenes.GameScene.Features.Helpers
{
    public class GameLoopSubscriber
    {
        private readonly IBallsService _ballsService;
        private readonly ILevelProgressService _levelProgressService;

        public GameLoopSubscriber(IBallsService ballsService, ILevelProgressService levelProgressService)
        {
            _ballsService = ballsService;
            _levelProgressService = levelProgressService;
        }
        
        public void SubscribeAll()
        {
            _levelProgressService.ProgressChanged += _ballsService.UpdateSpeedByProgress;
        }

        public void DescribeAll()
        {
            _levelProgressService.ProgressChanged -= _ballsService.UpdateSpeedByProgress;
        }
    }
}