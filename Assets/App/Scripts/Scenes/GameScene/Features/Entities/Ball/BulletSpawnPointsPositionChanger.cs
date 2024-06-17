using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball
{
    public sealed class BulletSpawnPointsPositionChanger : IBulletSpawnPointsPositionChanger
    {
        private readonly PlayerView _playerView;
        private float _previousWidth;

        public BulletSpawnPointsPositionChanger(PlayerView playerView)
        {
            _playerView = playerView;

            _previousWidth = _playerView.SpriteRenderer.bounds.size.x;
        }

        public void UpdateSpawnPositions()
        {
            float currentWidth = _playerView.SpriteRenderer.bounds.size.x;
            float widthDelta = currentWidth - _previousWidth;

            foreach (Transform pointTransform in _playerView.BulletsInitialPositions)
            {
                if (IsMiddlePoint(pointTransform))
                {
                    continue;
                }

                float direction = PointIsLeft(pointTransform) ? -1f : 1f;

                Vector3 position = pointTransform.position;
                position = new(position.x + widthDelta * direction / 2f, position.y);
                    
                pointTransform.position = position;
            }

            _previousWidth = currentWidth;
        }

        private static bool PointIsLeft(Transform pointTransform)
        {
            return pointTransform.localPosition.x < 0f;
        }

        private static bool IsMiddlePoint(Transform pointTransform)
        {
            return pointTransform.position.x == 0f;
        }
    }
}