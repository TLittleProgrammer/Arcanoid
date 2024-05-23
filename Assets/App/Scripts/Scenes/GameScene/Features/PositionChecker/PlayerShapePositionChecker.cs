using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PositionChecker
{
    public sealed class PlayerShapePositionChecker : IShapePositionChecker
    {
        private readonly float _minXPosition;
        private readonly float _maxXPosition;

        public PlayerShapePositionChecker(ISpriteRenderable spriteRenderable, IScreenInfoProvider screenInfoProvider)
        {
            float spriteSize = spriteRenderable.SpriteRenderer.bounds.size.x / 2f;

            _minXPosition = -screenInfoProvider.WidthInWorld / 2f + spriteSize;
            _maxXPosition = screenInfoProvider.WidthInWorld / 2f - spriteSize;
        }
        
        public bool CanChangePositionTo(Vector2 targetPosition)
        {
            if (targetPosition.x < _minXPosition || targetPosition.x > _maxXPosition)
            {
                return false;
            }

            return true;
        }
    }
}