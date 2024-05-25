using App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
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
        private readonly IShapePositionChecker _shapePositionChecker;
        private readonly IPlayerShapeMover _playerShapeMover;

        private float _initialBallSpeed;

        public BoostsActivator(
            SimpleDestroyService simpleDestroyService,
            IBoostContainer boostContainer,
            IBallFreeFlightMover ballMover,
            BoostsSettings boostsSettings,
            PlayerView playerView,
            IShapePositionChecker shapePositionChecker,
            IPlayerShapeMover playerShapeMover)
        {
            _simpleDestroyService = simpleDestroyService;
            _boostContainer = boostContainer;
            _ballMover = ballMover;
            _boostsSettings = boostsSettings;
            _playerView = playerView;
            _shapePositionChecker = shapePositionChecker;
            _playerShapeMover = playerShapeMover;

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
                
                _shapePositionChecker.ChangeShapeScale(1f);
                _playerView.transform.localScale = Vector3.one;
            }
            else if (boostId is BoostTypeId.PlayerShapeAddSpeed or BoostTypeId.PlayerShapeMinusSpeed)
            {
                _playerShapeMover.ChangeSpeed(1f);
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
                _playerView.SpriteRenderer.sprite = _boostsSettings.PlayerShapeSprites[BoostTypeId.PlayerShapeAddSize];
                
                _shapePositionChecker.ChangeShapeScale(_boostsSettings.AddPercent);
                _playerView.transform.localScale = Vector3.one * _boostsSettings.AddPercent;
            }
            else if (boostId is BoostTypeId.PlayerShapeMinusSize)
            {
                _playerView.SpriteRenderer.sprite = _boostsSettings.PlayerShapeSprites[BoostTypeId.PlayerShapeMinusSize];
                
                _shapePositionChecker.ChangeShapeScale(_boostsSettings.MinusPercent);
                _playerView.transform.localScale = Vector3.one * _boostsSettings.MinusPercent;
            }
            else if (boostId is BoostTypeId.PlayerShapeAddSpeed)
            {
                _playerShapeMover.ChangeSpeed(_boostsSettings.AddPercentSpeed);
            }
            else if (boostId is BoostTypeId.PlayerShapeMinusSpeed)
            {
                _playerShapeMover.ChangeSpeed(_boostsSettings.MinusPercentSpeed);
            }
        }
    }
}