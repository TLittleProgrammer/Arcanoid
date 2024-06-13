using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;

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

        public void Activate(IBoostDataProvider boostDataProvider)
        {
            FloatBoostData floatBoostData = (FloatBoostData)boostDataProvider;
            
            _ballsService.SetSpeedMultiplier(floatBoostData.Value);
        }

        public void Deactivate()
        {
            _ballsService.SetSpeedMultiplier(1f);
        }
    }
}