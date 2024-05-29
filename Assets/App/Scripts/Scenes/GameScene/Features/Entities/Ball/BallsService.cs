﻿using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Damage;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Input;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball
{
    public class BallsService : IBallsService, IActivable
    {
        private readonly IGetDamageService _getDamageService;
        private readonly BallView.Pool _ballViewPool;
        private readonly float _minBallYPosition;

        private List<IBallPositionChecker> _ballsPositionCheckers = new();
        private float _speedMultiplier;
        private float _levelProgress;
        private bool _redBallActivated;
        private float _lastSpeedMultiplier;
        private bool _isActive;

        public BallsService(
            IScreenInfoProvider screenInfoProvider,
            IGetDamageService getDamageService,
            IClickDetector clickDetector,
            BallView.Pool ballViewPool)
        {
            _getDamageService = getDamageService;
            _ballViewPool = ballViewPool;
            _minBallYPosition = -screenInfoProvider.HeightInWorld / 2f;
            _speedMultiplier = _lastSpeedMultiplier = 1f;
            _levelProgress = 0f;
            _isActive = true;
            
            Balls = new();
            clickDetector.MouseUp += OnMouseUp;
        }

        public Dictionary<BallView, IBallMovementService> Balls { get; set; }
        public event Action<BallView> BallAdded;

        public float SpeedMultiplier => _speedMultiplier;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value is false)
                {
                    _lastSpeedMultiplier = SpeedMultiplier;
                    SetSpeedMultiplier(0f);
                }
                else
                {
                    SetSpeedMultiplier(_lastSpeedMultiplier);
                }

                _isActive = value;
            }
        }
        

        public void AddBall(BallView ballView, bool isFreeFlight = false)
        {
            AddBallPositionChecker(ballView);
            BallAdded?.Invoke(ballView);

            ballView.RedBall.gameObject.SetActive(_redBallActivated);
            
            if (isFreeFlight)
            {
                Balls[ballView].UpdateSpeed(_levelProgress);
                Balls[ballView].SetSpeedMultiplier(_speedMultiplier);
                Balls[ballView].GoFly();
            }
        }

        public void Tick()
        {
            if (!IsActive)
                return;

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

        public void UpdateSpeedByProgress(float progress)
        {
            _levelProgress = progress;

            foreach ((var garbarge, IBallMovementService movementService) in Balls)
            {
                movementService.UpdateSpeed(_levelProgress);
            }
        }

        public void SetRedBall(bool activated)
        {
            _redBallActivated = activated;
            
            foreach ((BallView view, var garbarge) in Balls)
            {
                view.RedBall.gameObject.SetActive(activated);
            }
        }

        public void SetSticky(BallView view)
        {
            Balls[view].Sticky();
        }

        public void Fly(BallView view)
        {
            IBallMovementService movementService = Balls[view];

            if (!movementService.IsFreeFlight)
            {
                if (_speedMultiplier > 1f)
                {
                    view.TrailRenderer.enabled = true;
                }
                movementService.GoFly();
            }
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            _speedMultiplier = multiplier;

            bool needTrail = _speedMultiplier > 1f;
            
            foreach ((BallView view, IBallMovementService movementService) in Balls)
            {
                if (movementService.IsFreeFlight)
                {
                    view.TrailRenderer.enabled = needTrail;
                }
                
                movementService.SetSpeedMultiplier(_speedMultiplier);
            }
        }

        private void OnMouseUp()
        {
            foreach ((BallView view, IBallMovementService movementService) in Balls)
            {
                if (!movementService.IsFreeFlight)
                {
                    if (_speedMultiplier > 1f)
                    {
                        view.TrailRenderer.enabled = true;
                    }
                    
                    movementService.GoFly();
                }
            }
        }

        private void AddBallPositionChecker(BallView ballView)
        {
            var ballPositionChecker = new BallPositionChecker(ballView, _minBallYPosition);
            ballPositionChecker.BallFallen += OnBallFallen;

            _ballsPositionCheckers.Add(ballPositionChecker);
        }

        public void Reset()
        {
            BallView ballView = _ballViewPool.Spawn();
            var ballData = Balls.First(x => x.Key.Equals(ballView));

            ballView.TrailRenderer.enabled = false;
            ballData.Key.gameObject.SetActive(true);
            ballData.Value.Restart();
            
            AddBallPositionChecker(ballData.Key);
        }
    }
}