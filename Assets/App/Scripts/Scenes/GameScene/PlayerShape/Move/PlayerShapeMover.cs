using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Input;
using App.Scripts.Scenes.GameScene.PositionChecker;
using App.Scripts.Scenes.GameScene.Time;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.PlayerShape.Move
{
    public sealed class PlayerShapeMover : ITickable
    {
        private readonly IPositionable _playerPositionable;
        private readonly ITimeProvider _timeProvider;
        private readonly IInputService _inputService;
        private readonly IPositionChecker _positionChecker;

        private ShapeMoverSettings _shapeMoverSettings;

        public PlayerShapeMover(
            IPositionable playerPositionable,
            ITimeProvider timeProvider,
            IInputService inputService,
            IShapePositionChecker positionChecker,
            ShapeMoverSettings shapeMoverSettings)
        {
            _playerPositionable = playerPositionable;
            _timeProvider = timeProvider;
            _inputService = inputService;
            _positionChecker = positionChecker;
            _shapeMoverSettings = shapeMoverSettings;
        }
        
        public void Tick()
        {
            if (_inputService.UserClickDown)
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
            return Vector2.Lerp
            (
                _playerPositionable.Position,
                new(_inputService.CurrentMousePosition.x, _playerPositionable.Position.y),
                _timeProvider.DeltaTime * _shapeMoverSettings.Speed
            );
        }
    }
}