using App.Scripts.Scenes.GameScene.Features.Entities.Ball;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class BallMoverBoostActivator : IConcreteBoostActivator
    {
        private IBallsService _ballsService;
        private float _initialBallSpeed;

        public float SpeedMultiplier;
        
        public BallMoverBoostActivator(IBallsService ballsService)
        {
            _ballsService = ballsService;
        }

        public bool IsTimeableBoost => true;

        public void Activate()
        {
            _ballsService.SetSpeedMultiplier(SpeedMultiplier);
        }

        public void Deactivate()
        {
            _ballsService.SetSpeedMultiplier(1f);
        }
    }
}