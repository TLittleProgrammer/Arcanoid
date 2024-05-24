using App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;
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

        private float _initialBallSpeed;

        public BoostsActivator(
            SimpleDestroyService simpleDestroyService,
            IBoostContainer boostContainer,
            IBallFreeFlightMover ballMover,
            BoostsSettings boostsSettings)
        {
            _simpleDestroyService = simpleDestroyService;
            _boostContainer = boostContainer;
            _ballMover = ballMover;
            _boostsSettings = boostsSettings;

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
        }
    }
}