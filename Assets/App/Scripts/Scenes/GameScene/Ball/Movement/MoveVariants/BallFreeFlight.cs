using System;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.PositionChecker;
using App.Scripts.Scenes.GameScene.Settings;
using App.Scripts.Scenes.GameScene.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public class BallFreeFlight : IBallFreeFlightMover
    {
        private readonly BallFlyingSettings _settings;
        private readonly ITimeProvider _timeProvider;
        private readonly IBallPositionChecker _positionChecker;
        private readonly IPositionable _ballPositionable;
        
        private Vector3 _direction;

        public BallFreeFlight(
            IPositionable ballPositionable,
            BallFlyingSettings settings,
            ITimeProvider timeProvider,
            IBallPositionChecker positionChecker)
        {
            _ballPositionable = ballPositionable;
            _settings = settings;
            _timeProvider = timeProvider;
            _positionChecker = positionChecker;
        }

        public async UniTask AsyncInitialize(Vector2 param)
        {
            _direction = param.normalized;

            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            Vector2 targetPosition = _ballPositionable.Position + _direction * _settings.Speed * _timeProvider.DeltaTime;
            
            if (_positionChecker.CanChangePositionTo(targetPosition))
            {
                _ballPositionable.Position = targetPosition;
            }
            else
            {
                ChangeDirection();
            }
        }

        private void ChangeDirection()
        {
            _direction = _positionChecker.CurrentCollisionTypeId switch
            {
                CollisionTypeId.HorizontalSide => new Vector2(-_direction.x, _direction.y),
                CollisionTypeId.VerticalSide   => new Vector2(_direction.x, -_direction.y),
                
                _ => throw new ArgumentException($"Для типа коллизии {_positionChecker.CurrentCollisionTypeId} не определено поведение!")
            };
        }
    }
}