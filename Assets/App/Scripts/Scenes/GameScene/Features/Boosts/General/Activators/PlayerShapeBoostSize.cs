using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Movement;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class PlayerShapeBoostSize : IConcreteBoostActivator
    {
        private PlayerView _playerView;
        private IShapePositionChecker _shapePositionChecker;
        private IBulletMovement _bulletMovement;

        public float NewSize;

        public PlayerShapeBoostSize(
            PlayerView playerView,
            IShapePositionChecker shapePositionChecker,
            IBulletMovement bulletMovement)
        {
            _playerView = playerView;
            _shapePositionChecker = shapePositionChecker;
            _bulletMovement = bulletMovement;
        }
        
        public bool IsTimeableBoost => true;
        
        public void Activate(IBoostDataProvider boostDataProvider)
        {
            FloatBoostData floatBoostData = (FloatBoostData)boostDataProvider;
            UpdateWidth(floatBoostData.Value).Forget();
        }

        public async void Deactivate()
        {
            float currentWidth = _playerView.SpriteRenderer.size.x;
            await DOVirtual.Float(currentWidth, 1.5f, 0.5f, UpdateSpriteWidth);
                
            _shapePositionChecker.ChangeShapeScale();
            _bulletMovement.RecalculateSpawnPositions();
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