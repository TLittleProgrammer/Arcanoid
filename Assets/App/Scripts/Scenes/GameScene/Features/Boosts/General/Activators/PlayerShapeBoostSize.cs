using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Movement;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class PlayerShapeBoostSize : IConcreteBoostActivator
    {
        private readonly PlayerView _playerView;
        private readonly IShapePositionChecker _shapePositionChecker;
        private readonly BoostsSettings _boostsSettings;
        private readonly IBulletMovement _bulletMovement;

        public PlayerShapeBoostSize(
            PlayerView playerView,
            IShapePositionChecker shapePositionChecker,
            BoostsSettings boostsSettings,
            IBoostContainer boostContainer,
            IBulletMovement bulletMovement)
        {
            _playerView = playerView;
            _shapePositionChecker = shapePositionChecker;
            _boostsSettings = boostsSettings;
            _bulletMovement = bulletMovement;

            boostContainer.BoostEnded += OnBoostEnded;
        }

        public void Activate(BoostTypeId boostTypeId)
        {
            if (boostTypeId is BoostTypeId.PlayerShapeAddSize)
            {
                UpdateWidth(_boostsSettings.AddPercent).Forget();
            }
            else
            {
                UpdateWidth(_boostsSettings.MinusPercent).Forget();
            }
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

        private async void OnBoostEnded(BoostTypeId boostType)
        {
            if (boostType is BoostTypeId.PlayerShapeMinusSize or BoostTypeId.PlayerShapeAddSize)
            {
                float currentWidth = _playerView.SpriteRenderer.size.x;
                await DOVirtual.Float(currentWidth, 1.5f, 0.5f, UpdateSpriteWidth);
                
                _shapePositionChecker.ChangeShapeScale();
                _bulletMovement.RecalculateSpawnPositions();
            }
        }
    }
}