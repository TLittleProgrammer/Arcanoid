using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Ball.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Blocks;
using App.Scripts.Scenes.GameScene.Features.Camera;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using Cysharp.Threading.Tasks;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Ball
{
    public class BallsService : IBallsService, ITickable
    {
        private readonly IHealthContainer _healthContainer;
        private readonly IBlockShakeService _blockShakeService;
        private readonly ICameraService _cameraService;
        private readonly float _minBallYPosition;

        private List<IBallPositionChecker> _ballsPositionCheckers = new();
        
        public BallsService(
            IScreenInfoProvider screenInfoProvider,
            IHealthContainer healthContainer,
            IBlockShakeService blockShakeService,
            ICameraService cameraService)
        {
            _healthContainer = healthContainer;
            _blockShakeService = blockShakeService;
            _cameraService = cameraService;
            _minBallYPosition = -screenInfoProvider.HeightInWorld / 2f;
        }
        
        public void AddBall(IPositionable positionable)
        {
            var ballPositionChecker = new BallPositionChecker(positionable, _minBallYPosition);
            ballPositionChecker.BallFallen += OnBallFallen;
               
            _ballsPositionCheckers.Add(ballPositionChecker);
        }

        public void Tick()
        {
            foreach (IBallPositionChecker ballPositionChecker in _ballsPositionCheckers)
            {
                ballPositionChecker.Tick();
            }
        }

        private async void OnBallFallen(IPositionable ball)
        {
            await UniTask.Yield(PlayerLoopTiming.LastUpdate);
            _ballsPositionCheckers.Remove(_ballsPositionCheckers.First(x => x.BallPositionable.Equals(ball)));

            if (_ballsPositionCheckers.Count == 0)
            {
                GetDamage();
            } 
        }

        private void GetDamage()
        {
            _healthContainer.UpdateHealth(-1);
            _blockShakeService.Shake(_cameraService.Camera.transform);
        }
    }
}