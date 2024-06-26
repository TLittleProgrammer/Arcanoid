﻿using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.CustomData;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Damage;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Systems;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.Input;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball
{
    public class BallsService : IBallsService, IActivable, ILevelProgressSavable, IInitializeByLevelProgress
    {
        private readonly float _minBallYPosition;
        private readonly BallView.Pool _ballViewPool;
        private readonly IBallsMovementSystem _ballsMovementSystem;
        private readonly IMouseService _mouseService;
        private readonly IGetDamageService _getDamageService;

        private float _levelProgress;
        private float _speedMultiplier = 1f;
        private float _lastSpeedMultiplier = 1f;
        private bool _isActive = true;
        private bool _redBallActivated;
        private readonly List<IBallPositionChecker> _ballsPositionCheckers = new();

        public BallsService(
            IScreenInfoProvider screenInfoProvider,
            IGetDamageService getDamageService,
            IClickDetector clickDetector,
            BallView.Pool ballViewPool,
            IBallsMovementSystem ballsMovementSystem,
            IMouseService mouseService)
        {
            _getDamageService = getDamageService;
            _ballViewPool = ballViewPool;
            _ballsMovementSystem = ballsMovementSystem;
            _mouseService = mouseService;
            _minBallYPosition = -screenInfoProvider.HeightInWorld / 2f;

            Balls = new();
            clickDetector.MouseUp += OnMouseUp;
        }

        public List<BallView> Balls { get; }
        public event Action<BallView> BallAdded;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value is false)
                {
                    _lastSpeedMultiplier = _speedMultiplier;
                    SetSpeedMultiplier(0f);
                }
                else
                {
                    SetSpeedMultiplier(_lastSpeedMultiplier);
                }

                _isActive = value;
            }
        }

        public void Tick()
        {
            if (!IsActive)
                return;

            foreach (IBallPositionChecker ballPositionChecker in _ballsPositionCheckers)
            {
                ballPositionChecker.Tick();
            }
        }

        public void AddBall(BallView ballView, bool isFreeFlight = false)
        {
            _ballsMovementSystem.AddBall(ballView);
            Balls.Add(ballView);

            AddBallPositionCheckerForBall(ballView);
            
            BallAdded?.Invoke(ballView);

            ballView.RedBall.gameObject.SetActive(_redBallActivated);
            
            if (isFreeFlight)
            {
                var movementService = _ballsMovementSystem.GetMovementService(ballView);
                
                movementService.UpdateSpeed(_levelProgress);
                movementService.SetSpeedMultiplier(_speedMultiplier);
                movementService.GoFly();
            }
        }

        private async void OnBallFallen(IPositionable ball)
        {
            await UniTask.Yield(PlayerLoopTiming.LastUpdate);
            
            _ballsPositionCheckers.Remove(_ballsPositionCheckers.First(x => x.BallPositionable.Equals(ball)));
            BallView ballView = Balls.First(x => x.Position.Equals(ball.Position));

            if (!_ballViewPool.InactiveItems.Contains(ballView))
            {
                _ballViewPool.Despawn(ballView);
            }
            
            if (_ballsPositionCheckers.Count == 0)
            {
                _getDamageService.GetDamage(1);
                
                Reset();
            } 
        }

        public void UpdateSpeedByProgress(float progress)
        {
            _levelProgress = progress;

            foreach (BallView view in Balls)
            {
                var movementService = _ballsMovementSystem.GetMovementService(view);
                movementService.UpdateSpeed(_levelProgress);
            }
        }

        public void SetRedBall(bool activated)
        {
            _redBallActivated = activated;
            
            foreach (BallView view in Balls)
            {
                view.RedBall.gameObject.SetActive(activated);
            }
        }

        public void SetSticky(BallView view)
        {
            _ballsMovementSystem.GetMovementService(view).Sticky();
        }

        public void Fly(BallView view)
        {
            IBallMovementService movementService = _ballsMovementSystem.GetMovementService(view);

            if (!movementService.IsFreeFlight)
            {
                if (_speedMultiplier > 1f)
                {
                    view.TrailRenderer.enabled = true;
                }
                
                movementService.GoFly();
            }
        }

        public void DespawnAll()
        {
            foreach (BallView view in Balls)
            {
                if (view.gameObject.activeSelf)
                {
                    _ballViewPool.Despawn(view);
                }
            }
            
            Reset();
        }

        public bool BallIsSticked(BallView ballView)
        {
            return _ballsMovementSystem.GetMovementService(ballView).IsFreeFlight == false;
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            _speedMultiplier = multiplier;

            bool needTrail = _speedMultiplier > 1f;
            
            foreach (BallView view in Balls)
            {
                var movementService = _ballsMovementSystem.GetMovementService(view);
                
                if (movementService.IsFreeFlight)
                {
                    view.TrailRenderer.enabled = needTrail;
                }
                
                movementService.SetSpeedMultiplier(_speedMultiplier);
            }
        }

        private void OnMouseUp()
        {
            if (!IsActive || _mouseService.IsMouseOnRect())
            {
                return;
            }
            
            foreach (BallView view in Balls)
            {
                Fly(view);
            }
        }

        private void AddBallPositionCheckerForBall(BallView ballView)
        {
            var ballPositionChecker = new BallPositionChecker(ballView, _minBallYPosition);
            ballPositionChecker.BallFallen += OnBallFallen;

            _ballsPositionCheckers.Add(ballPositionChecker);
        }

        public void Restart()
        {
            _lastSpeedMultiplier = 1f;
            SetSpeedMultiplier(1f);
            UpdateSpeedByProgress(0f);
            
            DespawnAll();
        }

        private void Reset()
        {
            BallView ballView = _ballViewPool.Spawn();
            var firstBall = Balls.First(x => x.Equals(ballView));
            var movementService = _ballsMovementSystem.GetMovementService(firstBall);
            
            firstBall.TrailRenderer.enabled = false;
            firstBall.gameObject.SetActive(true);
            movementService.Restart();
            _ballsPositionCheckers.Clear();
            
            AddBallPositionCheckerForBall(firstBall);
        }

        public void SaveProgress(LevelDataProgress levelDataProgress)
        {
            BallsSaveData ballsSaveData = new();

            ballsSaveData.SpeedMultiplier = _speedMultiplier;
            ballsSaveData.BallDatas = new();

            foreach (BallView view in Balls)
            {
                if (view.gameObject.activeSelf)
                {
                    var ballMovementService = _ballsMovementSystem.GetMovementService(view);
                    
                    BallData data = new();
                    data.Velocity = new Float2(ballMovementService.Velocity.x, ballMovementService.Velocity.y);
                    data.Position = new PositionData
                    {
                        X = view.transform.position.x,
                        Y = view.transform.position.y,
                        Z = view.transform.position.z
                    };
                
                    data.IsFreeFlight = ballMovementService.IsFreeFlight;
                    ballsSaveData.BallDatas.Add(data);
                }
            }
            
            levelDataProgress.BallsData = ballsSaveData;
        }

        public void LoadProgress(LevelDataProgress levelDataProgress)
        {
            UpdateSpeedByProgress(levelDataProgress.ProgressedLevelData.Progress);
            _lastSpeedMultiplier = levelDataProgress.BallsData.SpeedMultiplier;
            List<(BallData, IBallMovementService, BallView)> movementServices = new();
            foreach (BallData ballData in levelDataProgress.BallsData.BallDatas)
            {
                BallView ballView = _ballViewPool.Spawn();
                _ballsMovementSystem.AddBall(ballView);

                IBallMovementService ballMovement = _ballsMovementSystem.GetMovementService(ballView);
                
                ballView.Position = new Vector3(
                    ballData.Position.X,
                    ballData.Position.Y,
                    ballData.Position.Z
                );
                
                movementServices.Add((ballData, ballMovement, ballView));
            }

            foreach ((BallData ballData, IBallMovementService ballMovement, BallView ballView)in movementServices)
            {
                if (ballData.IsFreeFlight)
                {
                    ballMovement.GoFly();
                    ballMovement.SetVelocity(new Vector2(ballData.Velocity.X, ballData.Velocity.Y));
                }
                else
                {
                    ballMovement.Sticky();
                }
                
                AddBallPositionCheckerForBall(ballView);
                
                Balls.Add(ballView);
                BallAdded?.Invoke(ballView);
            }
            
            SetSpeedMultiplier(levelDataProgress.BallsData.SpeedMultiplier);
        }
    }
}