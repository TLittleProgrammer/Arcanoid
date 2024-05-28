using App.Scripts.Scenes.GameScene.Features.Autopilot.Nodes;
using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Time;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Autopilot.Strategies
{
    public class SimpleMovingStrategy : IStrategy
    {
        private readonly IPositionable _playerView;
        private readonly IPositionable _ballView;
        private readonly IShapePositionChecker _shapePositionChecker;
        private readonly IPlayerShapeMover _playerShapeMover;
        private readonly ITimeProvider _timeProvider;

        public SimpleMovingStrategy(
            PlayerView playerView,
            BallView ballView,
            IShapePositionChecker shapePositionChecker,
            IPlayerShapeMover playerShapeMover,
            ITimeProvider timeProvider)
        {
            _playerView = playerView;
            _ballView = ballView;
            _shapePositionChecker = shapePositionChecker;
            _playerShapeMover = playerShapeMover;
            _timeProvider = timeProvider;
        }
        
        public NodeStatus Process()
        {
            if (Mathf.Abs(_playerView.Position.x - _ballView.Position.x) <= BehaviourTreeConstants.Epsilon)
            {
                return NodeStatus.Success;
            }

            Move();

            return NodeStatus.Running;
        }

        public void Reset()
        {
            
        }

        private void Move()
        {
            Vector2 targetPosition = CalculateTargetPosition();

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

        private Vector2 CalculateTargetPosition()
        {
            return Vector2.MoveTowards
            (
                _playerView.Position,
                new(_ballView.Position.x, _playerView.Position.y),
                _timeProvider.DeltaTime * _playerShapeMover.Speed
            );
        }
    }
}