﻿using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants
{
    public class BallFreeFlight : IBallFreeFlightMover
    {
        private readonly BallFlyingSettings _settings;
        private readonly ITimeProvider _timeProvider;
        private readonly IAngleCorrector _angleCorrector;
        private readonly IRigidablebody _ballRigidbody;
        private readonly float _maxSecondAngle;
        private readonly float _minSecondAngle;

        private float _speed;
        private float _multiplier;
        private Vector2 _lastDirection;

        public BallFreeFlight(
            BallView ballView,
            BallFlyingSettings settings,
            ITimeProvider timeProvider,
            IAngleCorrector angleCorrector)
        {
            _ballRigidbody = ballView;
            _settings = settings;
            _timeProvider = timeProvider;
            _angleCorrector = angleCorrector;

            _ballRigidbody.Collidered += OnCollidered;
            _timeProvider.TimeScaleChanged += OnTimeScaleChanged;

            _speed = _settings.Speed;
            _multiplier = 1f;
        }

        public async UniTask AsyncInitialize(Vector2 param)
        {
            Velocity = param.normalized * Speed;
            
            await UniTask.CompletedTask;
        }

        public Vector2 Velocity
        {
            get => _ballRigidbody.Rigidbody2D.velocity;
            set => _ballRigidbody.Rigidbody2D.velocity = value;
        }

        private float Speed => _speed * _multiplier;

        public void SetSpeed(float targetValue)
        {
            _speed = targetValue;
            Velocity = Velocity.normalized * Speed;
        }

        public void Reset()
        {
            _lastDirection = Velocity.normalized;
            Velocity = Vector2.zero;
        }

        public void SetSpeedMultiplier(float speedMultiplier)
        {
            _multiplier = speedMultiplier;

            if (speedMultiplier == 0f)
            {
                _lastDirection = Velocity.normalized;
                Velocity = Vector2.zero;
                
                return;
            }

            if (_lastDirection == Vector2.zero)
            {
                _lastDirection = Velocity.normalized;
            }

            if (Velocity.normalized == Vector2.zero)
            {
                Velocity = _lastDirection * Speed;
            }
            else
            {
                Velocity = Velocity.normalized * Speed;
            }
        }

        public void SetVelocity(Vector2 velocity)
        {
            _lastDirection = velocity;
            Velocity = velocity;
        }

        public void Restart()
        {
            Velocity = Vector2.zero;
            _ballRigidbody.Rigidbody2D.simulated = true;
        }

        private void OnTimeScaleChanged()
        {
            Velocity = Velocity.normalized * Speed * _timeProvider.TimeScale;
        }

        private void OnCollidered(BallView view, Collider2D collider)
        {
            float targetAngle = _angleCorrector.CorrectAngleByDirection(Velocity);
            UpdateVelocityByAngle(targetAngle);
        }

        private void UpdateVelocityByAngle(float targetAngle)
        {
            float speed = Velocity.magnitude;
            
            Velocity = new Vector2
            {
                x = speed * Mathf.Cos(targetAngle * Mathf.Deg2Rad),
                y = speed * Mathf.Sin(targetAngle * Mathf.Deg2Rad)
            };
        }
    }
}