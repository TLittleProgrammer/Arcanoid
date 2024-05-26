using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.MiniGun;
using App.Scripts.Scenes.GameScene.Features.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Settings;
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

        public void Activate(BoostTypeId boostTypeId)
        {
            if (boostTypeId is BoostTypeId.PlayerShapeAddSize)
            {
                _playerView.SpriteRenderer.sprite = _boostsSettings.PlayerShapeSprites[BoostTypeId.PlayerShapeAddSize];
                
                _shapePositionChecker.ChangeShapeScale(_boostsSettings.AddPercent);
                _playerView.transform.localScale = Vector3.one * _boostsSettings.AddPercent;
                
                _miniGunService.RecalculateSpawnPositions();
            }
            else
            {
                _playerView.SpriteRenderer.sprite = _boostsSettings.PlayerShapeSprites[BoostTypeId.PlayerShapeMinusSize];
                
                _shapePositionChecker.ChangeShapeScale(_boostsSettings.MinusPercent);
                _playerView.transform.localScale = Vector3.one * _boostsSettings.MinusPercent;
                
                _miniGunService.RecalculateSpawnPositions();
            }
        }

        private void OnBoostEnded(BoostTypeId boostType)
        {
            if (boostType is BoostTypeId.PlayerShapeMinusSize or BoostTypeId.PlayerShapeAddSize)
            {
                _playerView.SpriteRenderer.sprite = _boostsSettings.PlayerShapeSprites[BoostTypeId.None];
                
                _shapePositionChecker.ChangeShapeScale(1f);
                _playerView.transform.localScale = Vector3.one;
                
                _miniGunService.RecalculateSpawnPositions();
            }
        }
    }
}