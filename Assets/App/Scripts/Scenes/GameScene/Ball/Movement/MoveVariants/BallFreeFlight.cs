using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public class BallFreeFlight : IBallFreeFlightMover
    {
        private readonly BallFlyingSettings _settings;
        private readonly IRigidablebody _ballRigidbody;
        private readonly float _maxSecondAngle;
        private readonly float _minSecondAngle;

        private Vector3 _direction;
        private float _speed;

        public BallFreeFlight(IRigidablebody ballRigidbody, BallFlyingSettings settings)
        {
            _ballRigidbody = ballRigidbody;
            _settings = settings;
            _ballRigidbody.Collidered += OnCollidered;

            _speed = _settings.Speed;
            _maxSecondAngle = (180f - _settings.MaxAngle);
            _minSecondAngle = (180f - _settings.MinAngle);
        }

        private Vector2 Velocity
        {
            get => _ballRigidbody.Rigidbody2D.velocity;
            set => _ballRigidbody.Rigidbody2D.velocity = value;
        }

        public async UniTask AsyncInitialize(Vector2 param)
        {
            Velocity = param.normalized * _speed;

            await UniTask.CompletedTask;
        }

        public void UpdateSpeed(float addValue)
        {
            _speed += addValue;
        }

        public void Restart()
        {
            Velocity = Vector2.zero;
            _speed = _settings.Speed;
        }

        private async void OnCollidered(Collider2D collider)
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