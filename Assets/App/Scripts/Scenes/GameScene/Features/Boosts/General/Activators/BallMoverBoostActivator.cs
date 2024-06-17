using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class BallMoverBoostActivator : IConcreteBoostActivator
    {
        private readonly IBallsService _ballsService;
        private readonly FloatBoostData _boostDataProvider;
        
        private float _initialBallSpeed;
        
        public BallMoverBoostActivator(IBallsService ballsService, FloatBoostData boostDataProvider)
        {
            _ballsService = ballsService;
            _boostDataProvider = boostDataProvider;
        }

        public bool IsTimeableBoost => true;

        public void Activate()
        {
            _ballsService.SetSpeedMultiplier(_boostDataProvider.Value);
        }

        public void Deactivate()
        {
            _ballsService.SetSpeedMultiplier(1f);
        }
    }
}