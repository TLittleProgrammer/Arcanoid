using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Input;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Time;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PlayerShape.Move
{
    public sealed class PlayerShapeMover : IPlayerShapeMover
    {
        private readonly IPositionable _playerPositionable;
        private readonly ITimeProvider _timeProvider;
        private readonly IInputService _inputService;
        private readonly IShapePositionChecker _positionChecker;

        private ShapeMoverSettings _shapeMoverSettings;
        private readonly IRectMousePositionChecker _rectMousePositionChecker;
        private Vector3 _initialPosition;

        public PlayerShapeMover(
            IPositionable playerPositionable,
            ITimeProvider timeProvider,
            IInputService inputService,
            IShapePositionChecker positionChecker,
            ShapeMoverSettings shapeMoverSettings,
            IRectMousePositionChecker rectMousePositionChecker)
        {
            _playerPositionable = playerPositionable;
            _timeProvider = timeProvider;
            _inputService = inputService;
            _positionChecker = positionChecker;
            _shapeMoverSettings = shapeMoverSettings;
            _rectMousePositionChecker = rectMousePositionChecker;
            _initialPosition = _playerPositionable.Position;
        }
        
        public void Tick()
        {
            if (_inputService.UserClickDown && _rectMousePositionChecker.MouseOnRect(_inputService.CurrentMousePosition))
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