using App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts
{
    public class BoostsActivator : IBoostsActivator
    {
        private readonly SimpleDestroyService _simpleDestroyService;
        private readonly IBoostContainer _boostContainer;
        private readonly IBallFreeFlightMover _ballMover;
        private readonly BoostsSettings _boostsSettings;
        private readonly PlayerView _playerView;

        private float _initialBallSpeed;

        public BoostsActivator(
            SimpleDestroyService simpleDestroyService,
            IBoostContainer boostContainer,
            IBallFreeFlightMover ballMover,
            BoostsSettings boostsSettings,
            PlayerView playerView)
        {
            _simpleDestroyService = simpleDestroyService;
            _boostContainer = boostContainer;
            _ballMover = ballMover;
            _boostsSettings = boostsSettings;
            _playerView = playerView;

            _boostContainer.BoostEnded += OnBoostEnded;
            _initialBallSpeed = ballMover.GeneralSpeed;
        }

        public void Activate(BoostView view)
        {
            UseBoost(view.BoostTypeId);
            
            _boostContainer.AddActive(view.BoostTypeId);
            _simpleDestroyService.Destroy(view);
        }

        private void OnBoostEnded(BoostTypeId boostId)
        {
            if (boostId is BoostTypeId.BallAcceleration or BoostTypeId.BallSlowdown)
            {
                _ballMover.SetSpeed(_initialBallSpeed);
            }
            else if (boostId is BoostTypeId.PlayerShapeMinusSize or BoostTypeId.PlayerShapeAddSize)
            {
                _playerView.SpriteRenderer.sprite = _boostsSettings.PlayerShapeSprites[BoostTypeId.None];
                _playerView.transform.localScale = Vector3.one;
            }
        }

        private void UseBoost(BoostTypeId boostId)
        {
            if (boostId is BoostTypeId.BallAcceleration)
            {
                _ballMover.SetSpeed(_initialBallSpeed * _boostsSettings.AcceleratePercentFromAll);
            }
            else if (boostId is BoostTypeId.BallSlowdown)
            {
                _ballMover.SetSpeed(_initialBallSpeed * _boostsSettings.SlowDownPercentFromAll);
            }
            else if (boostId is BoostTypeId.PlayerShapeAddSize)
            {
                _playerView.transform.localScale = Vector3.one * _boostsSettings.AddPercent;
                _playerView.SpriteRenderer.sprite = _boostsSettings.PlayerShapeSprites[BoostTypeId.PlayerShapeAddSize];
            }
            else if (boostId is BoostTypeId.PlayerShapeMinusSize)
            {
                _playerView.transform.localScale = Vector3.one * _boostsSettings.MinusPercent;
                _playerView.SpriteRenderer.sprite = _boostsSettings.PlayerShapeSprites[BoostTypeId.PlayerShapeMinusSize];
            }
        }
    }
}