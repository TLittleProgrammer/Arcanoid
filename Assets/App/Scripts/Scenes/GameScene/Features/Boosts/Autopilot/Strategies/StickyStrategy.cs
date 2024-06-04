using App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Nodes;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Autopilot.Strategies
{
    public class StickyStrategy : IStrategy
    {
        private readonly IBallsService _ballsService;
        private readonly ILevelLoader _levelLoader;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IAutopilotMoveService _autopilotMoveService;
        private readonly PlayerView _playerView;
        private readonly IShapePositionChecker _shapePositionChecker;

        private bool _isMoving;
        private Vector3 _targetPosition;

        public StickyStrategy(
            IBallsService ballsService,
            ILevelLoader levelLoader,
            ILevelViewUpdater levelViewUpdater,
            IAutopilotMoveService autopilotMoveService,
            PlayerView playerView,
            IShapePositionChecker shapePositionChecker)
        {
            _ballsService = ballsService;
            _levelLoader = levelLoader;
            _levelViewUpdater = levelViewUpdater;
            _autopilotMoveService = autopilotMoveService;
            _playerView = playerView;
            _shapePositionChecker = shapePositionChecker;
        }
        
        public NodeStatus Process()
        {
            if (IsMoving(out NodeStatus status))
            {
                return status;
            }

            if (CheckBalls(out status))
            {
                return status;
            }

            Reset();
            return NodeStatus.Success;
        }

        private bool IsMoving(out NodeStatus success)
        {
            success = NodeStatus.Running;
            if (_isMoving)
            {
                _autopilotMoveService.Move(_targetPosition);

                if (
                    Mathf.Abs(_playerView.Position.x - _targetPosition.x) <= BehaviourTreeConstants.StickyEpsilon ||
                    !_shapePositionChecker.CanChangePositionTo(_targetPosition))
                {
                    FlyAllActiveBalls();

                    Reset();
                    success = NodeStatus.Success;
                    return true;
                }
                else
                {
                    Debug.Log($"CURRENT POSITION: {_playerView.Position}\nTARGET POSITION: {_targetPosition}");
                }

                return true;
            }

            return false;
        }

        private void FlyAllActiveBalls()
        {
            foreach ((BallView ballView, IBallMovementService movementService) in _ballsService.Balls)
            {
                if (ballView.gameObject.activeSelf)
                {
                    _ballsService.Fly(ballView);
                }
            }
        }

        private bool CheckBalls(out NodeStatus nodeStatus)
        {
            int activeBallsCounter = 0;
            int notFreeFlightBallsCounter = 0;

            CheckAllBalls(ref activeBallsCounter, ref notFreeFlightBallsCounter);

            if (activeBallsCounter == notFreeFlightBallsCounter)
            {
                _targetPosition = GetTargetPosition();
                _isMoving = true;

                nodeStatus = NodeStatus.Running;
                return true;
            }

            nodeStatus = NodeStatus.Success;
            return false;
        }

        private void Reset()
        {
            _targetPosition = Vector3.zero;
            _isMoving = false;
        }

        private Vector3 GetTargetPosition()
        {
            IEntityView choosedEntity = null;
            
            foreach (IEntityView entity in _levelLoader.Entities)
            {
                GridItemData gridItemData = _levelViewUpdater.LevelGridItemData[entity.GridPositionX, entity.GridPositionY];

                if (gridItemData.CanGetDamage && gridItemData.CurrentHealth > 0)
                {
                    if (choosedEntity is null)
                    {
                        choosedEntity = entity;
                        continue;
                    }

                    if (choosedEntity.BoostTypeId is BoostTypeId.None && entity.BoostTypeId is not BoostTypeId.None)
                    {
                        choosedEntity = entity;
                    }
                }
            }

            return choosedEntity is null ? Vector3.zero : choosedEntity.Position;
        }

        private void CheckAllBalls(ref int activeBallsCounter, ref int notFreeFlightBallsCounter)
        {
            foreach ((BallView ballView, IBallMovementService movementService) in _ballsService.Balls)
            {
                if (ballView.gameObject.activeSelf)
                {
                    activeBallsCounter++;

                    if (!movementService.IsFreeFlight)
                    {
                        notFreeFlightBallsCounter++;
                    }
                }
            }
        }
    }
}