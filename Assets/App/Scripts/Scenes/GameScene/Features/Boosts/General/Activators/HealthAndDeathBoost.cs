using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Settings;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class HealthAndDeathBoost : IConcreteBoostActivator
    {
        private readonly IHealthContainer _healthContainer;

        public HealthAndDeathBoost(IHealthContainer healthContainer)
        {
            _healthContainer = healthContainer;
        }
        
        public bool IsTimeableBoost => false;
        
        public void Activate(IBoostDataProvider boostDataProvider)
        {
            IntBoostData boostData = (IntBoostData)boostDataProvider;
            
            _healthContainer.UpdateHealth(boostData.Value, false);
        }

        public void Deactivate()
        {
            
        }
    }
}