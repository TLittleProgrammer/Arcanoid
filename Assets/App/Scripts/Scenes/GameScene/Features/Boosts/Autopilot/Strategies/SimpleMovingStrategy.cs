using System;
using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Nodes;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Time;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Strategies
{
    public class SimpleMovingStrategy : IStrategy
    {
        private readonly IPositionable _playerView;
        private readonly IShapePositionChecker _shapePositionChecker;
        private readonly IPlayerShapeMover _playerShapeMover;
        private readonly ITimeProvider _timeProvider;
        private readonly IBallsService _ballsService;

        public SimpleMovingStrategy(
            PlayerView playerView,
            IShapePositionChecker shapePositionChecker,
            IPlayerShapeMover playerShapeMover,
            ITimeProvider timeProvider,
            IBallsService ballsService)
        {
            _playerView = playerView;
            _shapePositionChecker = shapePositionChecker;
            _playerShapeMover = playerShapeMover;
            _timeProvider = timeProvider;
            _ballsService = ballsService;
        }
        
        public NodeStatus Process()
        {
            Vector3 bottomBallPosition = GetBottomBallPosition();

            if (Mathf.Abs(_playerView.Position.x - bottomBallPosition.x) <= BehaviourTreeConstants.Epsilon)
            {
                return NodeStatus.Success;
            }

            Move(bottomBallPosition);

            return NodeStatus.Running;
        }

        private Vector3 GetBottomBallPosition()
        {
            Vector3 position = Vector3.one * 999f;
            
            foreach ((BallView ballView, var garbarge) in _ballsService.Balls)
            {
                if (ballView.gameObject.activeSelf && ballView.transform.position.y < position.y)
                {
                    position = ballView.transform.position;
                }
            }

            return position;
        }

        public void Reset()
        {
            
        }

        private void Move(Vector3 bottomBallPosition)
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

        private Vector2 CalculateTargetPosition(Vector3 bottomBallPosition)
        {
            return Vector2.MoveTowards
            (
                _playerView.Position,
                new(bottomBallPosition.x, _playerView.Position.y),
                _timeProvider.DeltaTime * _playerShapeMover.Speed
            );
        }
    }
}