using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.ScreenInfo;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PositionChecker
{
    public sealed class BallPositionChecker : IBallPositionChecker
    {
        private readonly float _minXPosition;
        private readonly float _maxXPosition;
        private readonly float _minYPosition;
        private readonly float _maxYPosition;

        public BallPositionChecker(ISpriteRenderable spriteRenderable, IScreenInfoProvider screenInfoProvider)
        {
            float spriteSize = spriteRenderable.SpriteRenderer.bounds.size.x / 2f;

            _minXPosition = -screenInfoProvider.WidthInWorld / 2f + spriteSize;
            _maxXPosition = screenInfoProvider.WidthInWorld / 2f - spriteSize;
            _minYPosition = -screenInfoProvider.HeightInWorld / 2f + spriteSize;
            _maxYPosition = screenInfoProvider.HeightInWorld / 2f - spriteSize;
        }

        public CollisionTypeId CollisionTypeId { get; private set; }

        public bool CanChangePositionTo(Vector2 targetPosition)
        {
            if (targetPosition.x < _minXPosition || targetPosition.x > _maxXPosition)
            {
                CollisionTypeId = CollisionTypeId.HorizontalSide;
                return false;
            }
            
            if (targetPosition.y < _minYPosition || targetPosition.y > _maxYPosition)
            {
                CollisionTypeId = CollisionTypeId.VerticalSide;
                return false;
            }

            return true;
        }
    }
}