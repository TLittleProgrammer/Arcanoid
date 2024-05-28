using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Ball.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Damage;
using App.Scripts.Scenes.GameScene.Features.Factories;
using App.Scripts.Scenes.GameScene.Features.Input;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using Cysharp.Threading.Tasks;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Ball
{
    public class BallsService : IBallsService, ITickable
    {
        private readonly IGetDamageService _getDamageService;
        private readonly BallView.Pool _ballViewPool;
        private readonly float _minBallYPosition;

        private List<IBallPositionChecker> _ballsPositionCheckers = new();
        private float _speedMultiplier;

        public BallsService(
            IScreenInfoProvider screenInfoProvider,
            IGetDamageService getDamageService,
            IClickDetector clickDetector,
            BallView.Pool ballViewPool)
        {
            _getDamageService = getDamageService;
            _ballViewPool = ballViewPool;
            _minBallYPosition = -screenInfoProvider.HeightInWorld / 2f;
            _speedMultiplier = 1f;
            
            Balls = new();
            clickDetector.MouseUp += OnMouseUp;
        }

        public Dictionary<BallView, IBallMovementService> Balls { get; set; }

        public void AddBall(BallView ballView, bool isFreeFlight = false)
        {
            AddBallPositionChecker(ballView);

            if (isFreeFlight)
            {
                Balls[ballView].SetSpeedMultiplier(_speedMultiplier);
                Balls[ballView].GoFly();
            }
        }

        public void UpdateSpeedByProgress(float progress)
        {
            
        }

        public void Reset()
        {
            BallView ballView = _ballViewPool.Spawn();
            var ballData = Balls.First(x => x.Key.Equals(ballView));
            
            ballData.Key.gameObject.SetActive(true);
            ballData.Value.Restart();
            
            AddBallPositionChecker(ballData.Key);
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            _speedMultiplier = multiplier;

            foreach ((var garbarge, IBallMovementService movementService) in Balls)
            {
                movementService.SetSpeedMultiplier(_speedMultiplier);
            }
        }

        public void Tick()
        {
            foreach ((BallView ballView, IBallMovementService ballMovementService) in Balls)
            {
                if (ballView.gameObject.activeSelf)
                {
                    ballMovementService.Tick();
                }
            }
            
            foreach (IBallPositionChecker ballPositionChecker in _ballsPositionCheckers)
            {
                ballPositionChecker.Tick();
            }
        }

        private void OnMouseUp()
        {
            foreach ((var garbarge, IBallMovementService movementService) in Balls)
            {
                if (!movementService.IsFreeFlight)
                {
                    movementService.GoFly();
                }
            }
        }

        private async void OnBallFallen(IPositionable ball)
        {
            await UniTask.Yield(PlayerLoopTiming.LastUpdate);
            
            _ballsPositionCheckers.Remove(_ballsPositionCheckers.First(x => x.BallPositionable.Equals(ball)));
            BallView ballView = Balls.First(x => x.Key.Position.Equals(ball.Position)).Key;
            
            if (_ballsPositionCheckers.Count == 0)
            {
                _getDamageService.GetDamage(1);
                _ballViewPool.Despawn(ballView);
                
                Reset();
            } 
        }

        private void AddBallPositionChecker(BallView ballView)
        {
            var ballPositionChecker = new BallPositionChecker(ballView, _minBallYPosition);
            ballPositionChecker.BallFallen += OnBallFallen;

            _ballsPositionCheckers.Add(ballPositionChecker);
        }
    }
}