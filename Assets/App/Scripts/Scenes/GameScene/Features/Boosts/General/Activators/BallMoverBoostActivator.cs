using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Settings;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class BallMoverBoostActivator : IConcreteBoostActivator
    {
        private readonly BoostsSettings _boostsSettings;
        private readonly IBallsService _ballsService;
        private readonly float _initialBallSpeed;

        public BallMoverBoostActivator(BoostsSettings boostsSettings, IBallsService ballsService)
        {
            _boostsSettings = boostsSettings;
            _ballsService = ballsService;
        }

        public bool IsTimeableBoost => true;

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

        public void Deactivate()
        {
            _ballsService.SetSpeedMultiplier(1f);
        }
    }
}