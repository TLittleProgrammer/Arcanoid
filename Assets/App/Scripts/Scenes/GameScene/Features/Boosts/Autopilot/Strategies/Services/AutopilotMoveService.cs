using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Time;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Strategies
{
    public sealed class AutopilotMoveService : IAutopilotMoveService
    {
        private readonly PlayerView _playerView;
        private readonly IShapePositionChecker _shapePositionChecker;
        private readonly ITimeProvider _timeProvider;
        private readonly IPlayerShapeMover _playerShapeMover;

        public AutopilotMoveService(
            PlayerView playerView,
            IShapePositionChecker shapePositionChecker,
            ITimeProvider timeProvider,
            IPlayerShapeMover playerShapeMover)
        {
            _playerView = playerView;
            _shapePositionChecker = shapePositionChecker;
            _timeProvider = timeProvider;
            _playerShapeMover = playerShapeMover;
        }
        
        public void Move(Vector3 bottomBallPosition)
        {
            Vector2 targetPosition = CalculateTargetPosition(bottomBallPosition);

            TryMoveShape(targetPosition);
        }

        private void TryMoveShape(Vector2 targetPosition)
        {
            if (_shapePositionChecker.CanChangePositionTo(targetPosition))
            {
                _playerView.Position = targetPosition;
            }
            else
            {
                float targetXPosition = _playerView.Position.x < 0f ? _shapePositionChecker.MinX : _shapePositionChecker.MaxX;
                
                _playerView.Position = new Vector3(
                    targetXPosition,
                    _playerView.Position.y,
                    0f
                );
            }
        }

        private Vector2 CalculateTargetPosition(Vector3 bottomUsablePosition)
        {
            return Vector2.MoveTowards
            (
                _playerView.Position,
                new(bottomUsablePosition.x, _playerView.Position.y),
                _timeProvider.DeltaTime * _playerShapeMover.Speed
            );
        }
    }
}