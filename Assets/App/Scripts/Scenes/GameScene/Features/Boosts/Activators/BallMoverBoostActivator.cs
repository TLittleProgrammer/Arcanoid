using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Activators
{
    public sealed class BallMoverBoostActivator : IConcreteBoostActivator
    {
        private readonly IBoostContainer _boostContainer;
        private readonly BoostsSettings _boostsSettings;
        private readonly IBallsService _ballsService;
        private readonly float _initialBallSpeed;

        public BallMoverBoostActivator(IBoostContainer boostContainer, BoostsSettings boostsSettings, IBallsService ballsService)
        {
            _boostContainer = boostContainer;
            _boostsSettings = boostsSettings;
            _ballsService = ballsService;

            _boostContainer.BoostEnded += OnBoostEnded;
        }

        public void Activate(BoostTypeId boostTypeId)
        {
            float multiplier = boostTypeId switch
            {
                BoostTypeId.BallAcceleration => _boostsSettings.AcceleratePercentFromAll,
                BoostTypeId.BallSlowdown => _boostsSettings.SlowDownPercentFromAll,
                
                _ => 1f
            };

            _ballsService.SetSpeedMultiplier(multiplier);
        }

        private void OnBoostEnded(BoostTypeId boostType)
        {
            if (boostType is BoostTypeId.BallAcceleration or BoostTypeId.BallSlowdown)
            {
                _ballsService.SetSpeedMultiplier(1f);
            }
        }
    }
}