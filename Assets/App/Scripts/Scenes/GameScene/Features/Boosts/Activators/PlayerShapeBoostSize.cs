using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.MiniGun;
using App.Scripts.Scenes.GameScene.Features.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Helpers
{
    public sealed class PlayerShapeBoostSize : IConcreteBoostActivator
    {
        private readonly PlayerView _playerView;
        private readonly IShapePositionChecker _shapePositionChecker;
        private readonly BoostsSettings _boostsSettings;
        private readonly IBoostContainer _boostContainer;
        private readonly IMiniGunService _miniGunService;

        public PlayerShapeBoostSize(
            PlayerView playerView,
            IShapePositionChecker shapePositionChecker,
            BoostsSettings boostsSettings,
            IBoostContainer boostContainer,
            IMiniGunService miniGunService)
        {
            _playerView = playerView;
            _shapePositionChecker = shapePositionChecker;
            _boostsSettings = boostsSettings;
            _boostContainer = boostContainer;
            _miniGunService = miniGunService;

            _boostContainer.BoostEnded += OnBoostEnded;
        }

        public async void Activate(BoostTypeId boostTypeId)
        {
            if (boostTypeId is BoostTypeId.PlayerShapeAddSize)
            {
                float currentWidth = _playerView.SpriteRenderer.size.x;
                await DOVirtual.Float(currentWidth, _boostsSettings.AddPercent, 0.5f, UpdateSpriteWidth);
                _shapePositionChecker.ChangeShapeScale();
                
                _miniGunService.RecalculateSpawnPositions();
            }
            else
            {
                float currentWidth = _playerView.SpriteRenderer.size.x;
                await DOVirtual.Float(currentWidth, _boostsSettings.MinusPercent, 0.5f, UpdateSpriteWidth);
                _shapePositionChecker.ChangeShapeScale();
                
                _miniGunService.RecalculateSpawnPositions();
            }
        }

        private void UpdateSpriteWidth(float value)
        {
            var spriteRendererSize = _playerView.SpriteRenderer.size;
            spriteRendererSize.x = value;

            _playerView.SpriteRenderer.size = spriteRendererSize;
            _playerView.BoxCollider2D.size = spriteRendererSize;
            
            _shapePositionChecker.ChangeShapeScale();
        }

        private void OnBoostEnded(BoostTypeId boostType)
        {
            if (boostType is BoostTypeId.PlayerShapeMinusSize or BoostTypeId.PlayerShapeAddSize)
            {
                float currentWidth = _playerView.SpriteRenderer.size.x;
                DOVirtual.Float(currentWidth, 1.5f, 0.5f, UpdateSpriteWidth);
                
                _shapePositionChecker.ChangeShapeScale();
                
                _miniGunService.RecalculateSpawnPositions();
            }
        }
    }
}