using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Input;
using App.Scripts.Scenes.GameScene.Time;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.PlayerShape.Move
{
    public sealed class PlayerShapeMover : ITickable
    {
        private readonly ITransformable _playerTransformable;
        private readonly ITimeProvider _timeProvider;
        private readonly IInputService _inputService;
        private readonly IPlayerPositionChecker _positionChecker;

        private ShapeMoverSettings _shapeMoverSettings;

        public PlayerShapeMover(
            ITransformable playerTransformable,
            ITimeProvider timeProvider,
            IInputService inputService,
            IPlayerPositionChecker positionChecker,
            ShapeMoverSettings shapeMoverSettings)
        {
            _playerTransformable = playerTransformable;
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
                _playerTransformable.Position = targetPosition;
            }
        }

        private Vector2 CalculateTargetPosition()
        {
            return Vector2.Lerp
            (
                _playerTransformable.Position,
                new(_inputService.CurrentMousePosition.x, _playerTransformable.Position.y),
                _timeProvider.DeltaTime * _shapeMoverSettings.Speed
            );
        }
    }
}