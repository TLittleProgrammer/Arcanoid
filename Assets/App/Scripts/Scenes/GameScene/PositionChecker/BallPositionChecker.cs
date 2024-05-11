using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Containers;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Levels.View;
using App.Scripts.Scenes.GameScene.ScreenInfo;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PositionChecker
{
    public sealed class BallPositionChecker : IBallPositionChecker
    {
        private readonly IContainer<IBoxColliderable2D> _collidersContainer;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly float _minXPosition;
        private readonly float _maxXPosition;
        private readonly float _minYPosition;
        private readonly float _maxYPosition;
        private readonly float _ballRadius;

        public BallPositionChecker(
            ISpriteRenderable ballRenderable,
            IScreenInfoProvider screenInfoProvider,
            IContainer<IBoxColliderable2D> collidersContainer,
            ILevelViewUpdater levelViewUpdater)
        {
            _collidersContainer = collidersContainer;
            _levelViewUpdater = levelViewUpdater;

            _ballRadius = ballRenderable.SpriteRenderer.bounds.size.x / 2f;
            
            _minXPosition = -screenInfoProvider.WidthInWorld / 2f + _ballRadius;
            _maxXPosition = screenInfoProvider.WidthInWorld / 2f - _ballRadius;
            _minYPosition = -screenInfoProvider.HeightInWorld / 2f + _ballRadius;
            _maxYPosition = screenInfoProvider.HeightInWorld / 2f - _ballRadius;
        }

        public CollisionTypeId CurrentCollisionTypeId { get; private set; }

        public bool CanChangePositionTo(Vector2 targetPosition)
        {
            if (!CheckHorizontalSides(targetPosition)) return false;
            if (!CheckVerticalSides(targetPosition)) return false;
            if (!CheckCollisionTriggers(targetPosition)) return false;
            

            return true;
        }

        private bool CheckCollisionTriggers(Vector2 targetPosition)
        {
            foreach (IBoxColliderable2D boxColliderable in _collidersContainer.GetItems())
            {
                if (IsTriggered(boxColliderable.BoxCollider2D, targetPosition))
                {
                    UpdateView(boxColliderable);
                    
                    Bounds bounds = boxColliderable.BoxCollider2D.bounds;

                    if (targetPosition.y >= bounds.min.y && targetPosition.y <= bounds.max.y)
                    {
                        CurrentCollisionTypeId = CollisionTypeId.HorizontalSide;
                        return false;
                    }

                    CurrentCollisionTypeId = CollisionTypeId.VerticalSide;
                    return false;
                }
            }

            return true;
        }

        private void UpdateView(IBoxColliderable2D boxColliderableBoxCollider2D)
        {
            if (boxColliderableBoxCollider2D is EntityView entityView)
            {
                _levelViewUpdater.UpdateVisual(entityView);
            }
        }

        private bool IsTriggered(BoxCollider2D boxColliderableBoxCollider2D, Vector2 targetPosition)
        {
            return boxColliderableBoxCollider2D.OverlapPoint(targetPosition + new Vector2(_ballRadius, _ballRadius)) ||
                   boxColliderableBoxCollider2D.OverlapPoint(targetPosition + new Vector2(-_ballRadius, _ballRadius)) ||
                   boxColliderableBoxCollider2D.OverlapPoint(targetPosition + new Vector2(-_ballRadius, -_ballRadius)) ||
                   boxColliderableBoxCollider2D.OverlapPoint(targetPosition + new Vector2(_ballRadius, -_ballRadius));
        }

        private bool CheckVerticalSides(Vector2 targetPosition)
        {
            if (targetPosition.y < _minYPosition || targetPosition.y > _maxYPosition)
            {
                CurrentCollisionTypeId = CollisionTypeId.VerticalSide;
                return false;
            }

            return true;
        }

        private bool CheckHorizontalSides(Vector2 targetPosition)
        {
            if (targetPosition.x < _minXPosition || targetPosition.x > _maxXPosition)
            {
                CurrentCollisionTypeId = CollisionTypeId.HorizontalSide;
                return false;
            }

            return true;
        }
    }
}