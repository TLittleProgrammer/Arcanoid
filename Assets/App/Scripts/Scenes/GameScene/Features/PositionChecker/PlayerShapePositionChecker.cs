using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PositionChecker
{
    public sealed class PlayerShapePositionChecker : IShapePositionChecker
    {
        private readonly ISpriteRenderable _spriteRenderable;
        private readonly IScreenInfoProvider _screenInfoProvider;

        private float _minXPosition;
        private float _maxXPosition;

        public PlayerShapePositionChecker(ISpriteRenderable spriteRenderable, IScreenInfoProvider screenInfoProvider)
        {
            _spriteRenderable = spriteRenderable;
            _screenInfoProvider = screenInfoProvider;

            ChangeShapeScale();
        }

        public float MinX => _minXPosition;
        public float MaxX => _maxXPosition;

        public void ChangeShapeScale()
        {
            var bounds = _spriteRenderable.SpriteRenderer.size;
            _minXPosition = -_screenInfoProvider.WidthInWorld / 2f + bounds.x / 2f;
            _maxXPosition = _screenInfoProvider.WidthInWorld / 2f - bounds.x / 2f;
        }

        public bool CanChangePositionTo(Vector2 to)
        {
            if (to.x < _minXPosition || to.x > _maxXPosition)
            {
                return false;
            }

            return true;
        }
    }
}