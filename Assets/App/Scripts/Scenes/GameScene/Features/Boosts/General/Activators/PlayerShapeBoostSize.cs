using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Movement;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.PositionChecker;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class PlayerShapeBoostSize : IConcreteBoostActivator
    {
        private readonly PlayerView _playerView;
        private readonly IShapePositionChecker _shapePositionChecker;
        private readonly IBulletMovement _bulletMovement;
        private readonly float _defaultShapeSize;

        public PlayerShapeBoostSize(
            PlayerView playerView,
            IShapePositionChecker shapePositionChecker,
            IBulletMovement bulletMovement)
        {
            _playerView = playerView;
            _shapePositionChecker = shapePositionChecker;
            _bulletMovement = bulletMovement;

            _defaultShapeSize = _playerView.BoxCollider2D.size.x;
        }
        
        public bool IsTimeableBoost => true;
        
        public void Activate(IBoostDataProvider boostDataProvider)
        {
            FloatBoostData floatBoostData = (FloatBoostData)boostDataProvider;
            UpdateWidth(floatBoostData.Value).Forget();
        }

        public void Deactivate()
        {
            UpdateWidth(_defaultShapeSize).Forget();
        }

        private async UniTask UpdateWidth(float to)
        {
            float currentWidth = _playerView.SpriteRenderer.size.x;
            await DOVirtual.Float(currentWidth, to, 0.5f, UpdateSpriteWidth);
            
            _shapePositionChecker.ChangeShapeScale();
            _bulletMovement.RecalculateSpawnPositions();
        }

        private void UpdateSpriteWidth(float value)
        {
            var spriteRendererSize = _playerView.SpriteRenderer.size;
            spriteRendererSize.x = value;

            _playerView.SpriteRenderer.size = spriteRendererSize;
            _playerView.BoxCollider2D.size = spriteRendererSize;
            
            _shapePositionChecker.ChangeShapeScale();
        }
    }
}