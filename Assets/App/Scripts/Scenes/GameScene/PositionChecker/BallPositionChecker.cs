using System;
using App.Scripts.Scenes.GameScene.Collisions;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Containers;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.PlayerShape;
using App.Scripts.Scenes.GameScene.ScreenInfo;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PositionChecker
{
    public sealed class BallPositionChecker : IBallPositionChecker
    {
        private readonly IContainer<IBoxColliderable2D> _collidersContainer;
        private readonly ICollisionService<EntityView> _collisionService;
        private readonly float _minXPosition;
        private readonly float _maxXPosition;
        private readonly float _minYPosition;
        private readonly float _maxYPosition;
        private readonly float _ballRadius;

        public BallPositionChecker(
            ISpriteRenderable ballRenderable,
            IScreenInfoProvider screenInfoProvider,
            IContainer<IBoxColliderable2D> collidersContainer,
            ICollisionService<EntityView> collisionService)
        {
            _collidersContainer = collidersContainer;
            _collisionService = collisionService;

            _ballRadius = ballRenderable.SpriteRenderer.bounds.size.x / 2f;
            
            _minXPosition = -screenInfoProvider.WidthInWorld / 2f + _ballRadius;
            _maxXPosition = screenInfoProvider.WidthInWorld / 2f - _ballRadius;
            _minYPosition = -screenInfoProvider.HeightInWorld / 2f + _ballRadius;
            _maxYPosition = screenInfoProvider.HeightInWorld / 2f - _ballRadius;
        }

        public CollisionTypeId CurrentCollisionTypeId { get; private set; }

        public bool CanChangePositionTo(Vector2 targetPosition, ref Vector3 direction)
        {
            if (!CheckHorizontalSides(targetPosition, ref direction)) return false;
            if (!CheckVerticalSides(targetPosition, ref direction)) return false;
            if (!CheckCollisionTriggers(targetPosition, ref direction)) return false;
            

            return true;
        }

        private bool CheckCollisionTriggers(Vector2 targetPosition, ref Vector3 direction)
        {
            foreach (IBoxColliderable2D boxColliderable in _collidersContainer.GetItems())
            {
                if (IsTriggered(boxColliderable.BoxCollider2D, targetPosition))
                {
                    if (boxColliderable is PlayerView playerView)
                    {
                        float newAngle = (-(targetPosition.x - playerView.transform.position.x) * 60f) + 90f;

                        direction.x = direction.x * Mathf.Cos(newAngle) - direction.y * Mathf.Sin(newAngle);
                        direction.y = direction.y * Mathf.Cos(newAngle) + direction.x * Mathf.Sin(newAngle);
                        direction.Normalize();
                    }
                    
                    return NewLogic(targetPosition, boxColliderable, ref direction);
                }
            }

            return true;
        }

        private bool NewLogic(Vector2 targetPosition, IBoxColliderable2D boxColliderable, ref Vector3 direction)
        {
            UpdateView(boxColliderable);
            
            var delta = new Vector3(targetPosition.x, targetPosition.y, 0f) - boxColliderable.BoxCollider2D.transform.position;
            delta.x /= 2f;

            if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
            {
                if (Math.Abs(Mathf.Sign(-direction.x) - Mathf.Sign(delta.x)) < 0.001f)
                {
                    direction.x *= -1;
                    CurrentCollisionTypeId = CollisionTypeId.HorizontalSide;
                    return false;
                }
            }
            
            if (Math.Abs(Mathf.Sign(-direction.y) - Mathf.Sign(delta.y)) < 0.001f)
            {
                direction.y *= -1;
            }
            
            CurrentCollisionTypeId = CollisionTypeId.VerticalSide;
            return false;
        }

        private void UpdateView(IBoxColliderable2D boxColliderableBoxCollider2D)
        {
            if (boxColliderableBoxCollider2D is EntityView entityView)
            {
                _collisionService.Collide(entityView);
            }
        }

        private bool IsTriggered(BoxCollider2D boxColliderableBoxCollider2D, Vector2 targetPosition)
        {
            return boxColliderableBoxCollider2D.OverlapPoint(targetPosition + new Vector2(_ballRadius, _ballRadius)) ||
                   boxColliderableBoxCollider2D.OverlapPoint(targetPosition + new Vector2(-_ballRadius, _ballRadius)) ||
                   boxColliderableBoxCollider2D.OverlapPoint(targetPosition + new Vector2(-_ballRadius, -_ballRadius)) ||
                   boxColliderableBoxCollider2D.OverlapPoint(targetPosition + new Vector2(_ballRadius, -_ballRadius));
        }

        private bool CheckVerticalSides(Vector2 targetPosition, ref Vector3 direction)
        {
            if (targetPosition.y > _maxYPosition)
            {
                CurrentCollisionTypeId = CollisionTypeId.VerticalSide;
                direction.y *= -1;
                return false;
            }

            if (targetPosition.y <= _minYPosition)
            {
                CurrentCollisionTypeId = CollisionTypeId.BottomVerticalSide;
                return false;
            }

            return true;
        }

        private bool CheckHorizontalSides(Vector2 targetPosition, ref Vector3 direction)
        {
            if (targetPosition.x < _minXPosition || targetPosition.x > _maxXPosition)
            {
                CurrentCollisionTypeId = CollisionTypeId.HorizontalSide;
                direction.x *= -1;
                return false;
            }

            return true;
        }
    }
}