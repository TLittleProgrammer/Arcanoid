using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.ScreenInfo;
using App.Scripts.Scenes.GameScene.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Walls
{
    public class WallLoader : IWallLoader
    {
        private readonly BoxCollidersAroundScreenSettings _settings;
        private readonly IBoxColliderable2D _prefab;
        private readonly IScreenInfoProvider _screenInfoProvider;

        private float _halfWidth;
        private float _halfHeight;
        
        public WallLoader(BoxCollidersAroundScreenSettings settings, IBoxColliderable2D prefab, IScreenInfoProvider screenInfoProvider)
        {
            _settings = settings;
            _prefab = prefab;
            _screenInfoProvider = screenInfoProvider;

            _halfWidth  = _screenInfoProvider.WidthInWorld / 2f;
            _halfHeight = screenInfoProvider.HeightInWorld / 2f;
        }
        
        public async UniTask AsyncInitialize()
        {
            foreach (BoxColliderData data in _settings.BoxColliderDatas)
            {
                BoxCollider2D collider = Object.Instantiate(_prefab.BoxCollider2D);

                UpdateSize(data, collider);
                UpdatePosition(collider, data);
            }

            await UniTask.CompletedTask;
        }

        private void UpdatePosition(BoxCollider2D collider, BoxColliderData data)
        {
            float targetX;
            float targetY;
            
            CalculateTargetPosition(out targetX, data.HorizontalCenter, _halfWidth, _screenInfoProvider.WidthInWorld);
            CalculateTargetPosition(out targetY, data.VerticalCenter, _halfHeight, _screenInfoProvider.HeightInWorld);
            
            Vector3 targetPosition = new Vector3
            {
                x = targetX,
                y = targetY,
                z = 0f,
            };
            
            collider.transform.position = targetPosition;
        }

        private void CalculateTargetPosition(out float targetValue, float center, float half, float inWorld)
        {
            if (center < 0f)
            {
                targetValue = -half + inWorld * center;
            }
            else if (center > 1f)
            {
                targetValue = half + inWorld * (Mathf.Abs(1f - center));
            }
            else
            {
                targetValue = Mathf.Lerp(-half, half, center);
            }
        }

        private void UpdateSize(BoxColliderData data, BoxCollider2D collider)
        {
            float height = _screenInfoProvider.HeightInWorld * data.VerticalSize;
            float width = _screenInfoProvider.WidthInWorld * data.HorizontalSize;

            collider.size = new(width, height);
        }
    }
}