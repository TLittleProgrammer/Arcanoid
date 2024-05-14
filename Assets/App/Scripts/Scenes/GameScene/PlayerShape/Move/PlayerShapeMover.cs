using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Constants;
using App.Scripts.Scenes.GameScene.Input;
using App.Scripts.Scenes.GameScene.PositionChecker;
using App.Scripts.Scenes.GameScene.States;
using App.Scripts.Scenes.GameScene.Time;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.PlayerShape.Move
{
    public sealed class PlayerShapeMover : IPlayerShapeMover
    {
        private readonly IPositionable _playerPositionable;
        private readonly ITimeProvider _timeProvider;
        private readonly IInputService _inputService;
        private readonly IPositionChecker _positionChecker;

        private ShapeMoverSettings _shapeMoverSettings;
        private readonly IStateMachine _stateMachine;
        private Vector3 _initialPosition;

        public PlayerShapeMover(
            IPositionable playerPositionable,
            ITimeProvider timeProvider,
            IInputService inputService,
            IShapePositionChecker positionChecker,
            ShapeMoverSettings shapeMoverSettings,
            [Inject(Id = BindingConstants.GameStateMachine)] IStateMachine stateMachine)
        {
            _playerPositionable = playerPositionable;
            _timeProvider = timeProvider;
            _inputService = inputService;
            _positionChecker = positionChecker;
            _shapeMoverSettings = shapeMoverSettings;
            _stateMachine = stateMachine;
            _initialPosition = _playerPositionable.Position;
        }
        
        public void Tick()
        {
            if (_stateMachine.CurrentState is GameLoopState && _inputService.UserClickDown)
            {
                Vector2 targetPosition = CalculateTargetPosition();

                TryMoveShape(targetPosition);
            }
        }

        private void TryMoveShape(Vector2 targetPosition)
        {
            if (_positionChecker.CanChangePositionTo(targetPosition))
            {
                _playerPositionable.Position = targetPosition;
            }
        }

        private Vector2 CalculateTargetPosition()
        {
            return Vector2.MoveTowards
            (
                _playerPositionable.Position,
                new(_inputService.CurrentMousePosition.x, _playerPositionable.Position.y),
                _timeProvider.DeltaTime * _shapeMoverSettings.Speed
            );
        }

        public void Restart()
        {
            _playerPositionable.Position = _initialPosition;
        }
    }
}