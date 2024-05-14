using System;
using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Constants;
using App.Scripts.Scenes.GameScene.Healthes;
using App.Scripts.Scenes.GameScene.PositionChecker;
using App.Scripts.Scenes.GameScene.Settings;
using App.Scripts.Scenes.GameScene.States;
using App.Scripts.Scenes.GameScene.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public class BallFreeFlight : IBallFreeFlightMover
    {
        private readonly BallFlyingSettings _settings;
        private readonly ITimeProvider _timeProvider;
        private readonly IBallPositionChecker _positionChecker;
        private readonly IStateMachine _stateMachine;
        private readonly IHealthContainer _healthContainer;
        private readonly IPositionable _ballPositionable;
        
        private Vector3 _direction;
        private float _speed;

        public BallFreeFlight(
            IPositionable ballPositionable,
            BallFlyingSettings settings,
            ITimeProvider timeProvider,
            IBallPositionChecker positionChecker,
            [Inject(Id = BindingConstants.GameStateMachine)] IStateMachine stateMachine,
            IHealthContainer healthContainer)
        {
            _ballPositionable = ballPositionable;
            _settings = settings;
            _timeProvider = timeProvider;
            _positionChecker = positionChecker;
            _stateMachine = stateMachine;
            _healthContainer = healthContainer;

            _speed = _settings.Speed;
        }

        public async UniTask AsyncInitialize(Vector2 param)
        {
            _direction = param.normalized;

            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            if (_stateMachine.CurrentState is not GameLoopState)
                return;
            
            Vector2 targetPosition = _ballPositionable.Position + _direction * _speed * _timeProvider.DeltaTime;
            
            if (_positionChecker.CanChangePositionTo(targetPosition))
            {
                _ballPositionable.Position = targetPosition;
            }
            else
            {
                if (_positionChecker.CurrentCollisionTypeId is CollisionTypeId.BottomVerticalSide)
                {
                    _healthContainer.UpdateHealth(-1);
                     return;
                }
                ChangeDirection();
            }
        }

        public void UpdateSpeed(float addValue)
        {
            _speed += addValue;
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

        public void Restart()
        {
            _speed = _settings.Speed;
        }
    }
}