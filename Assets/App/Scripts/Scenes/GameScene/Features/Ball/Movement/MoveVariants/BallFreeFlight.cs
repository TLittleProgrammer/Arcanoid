﻿using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants
{
    public class BallFreeFlight : IBallFreeFlightMover
    {
        private readonly BallFlyingSettings _settings;
        private readonly ITimeProvider _timeProvider;
        private readonly IRigidablebody _ballRigidbody;
        private readonly float _maxSecondAngle;
        private readonly float _minSecondAngle;

        private float _speed;
        private float _multiplier;
        private float _constantSpeed;
        private Vector2 _lastDirection;

        public BallFreeFlight(BallView ballView, BallFlyingSettings settings, ITimeProvider timeProvider)
        {
            _ballRigidbody = ballView;
            _settings = settings;
            _timeProvider = timeProvider;
            
            _ballRigidbody.Collidered += OnCollidered;
            _timeProvider.TimeScaleChanged += OnTimeScaleChanged;

            _speed = _settings.Speed;
            _multiplier = 1f;
            _constantSpeed  = _speed;
            _maxSecondAngle = 180f - _settings.MaxAngle;
            _minSecondAngle = 180f - _settings.MinAngle;
        }

        public async UniTask AsyncInitialize(Vector2 param)
        {
            Velocity = param.normalized * Speed;
            
            await UniTask.CompletedTask;
        }

        private Vector2 Velocity
        {
            get => _ballRigidbody.Rigidbody2D.velocity;
            set => _ballRigidbody.Rigidbody2D.velocity = value;
        }

        private float Speed => _speed * _multiplier;
        
        public float ConstantSpeed => _constantSpeed;

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

        public void Continue()
        {
            Velocity = _lastDirection * Speed;
        }

        public void SetSpeedMultiplier(float speedMultiplier)
        {
            _multiplier = speedMultiplier;

            Velocity = Velocity.normalized * Speed;
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

        private async void OnCollidered(BallView view, Collider2D collider)
        {
            await UniTask.WaitForFixedUpdate();
            
            ChangeAngleForDirection();
        }

        private void ChangeAngleForDirection()
        {
            float currentAngle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
            float absCurrentAngle = Mathf.Abs(currentAngle);

            if (absCurrentAngle < _settings.MinAngle)
            {
                UpdateVelocity(currentAngle > 0 ? _settings.MinAngle : -_settings.MinAngle);
            }
            else if (absCurrentAngle > _settings.MaxAngle && _maxSecondAngle > absCurrentAngle)
            {
                UpdateVelocity(currentAngle > 0 ? _settings.MaxAngle : -_settings.MaxAngle);
            }
            else if (absCurrentAngle > _minSecondAngle)
            {
                UpdateVelocity(currentAngle > 0 ? _minSecondAngle : -_minSecondAngle);
            }
        }

        private void UpdateVelocity(float targetAngle)
        {
            float speed = Velocity.magnitude;
            Velocity = new Vector2()
            {
                x = speed * Mathf.Cos(targetAngle * Mathf.Deg2Rad),
                y = speed * Mathf.Sin(targetAngle * Mathf.Deg2Rad)
            };
        }
    }
}