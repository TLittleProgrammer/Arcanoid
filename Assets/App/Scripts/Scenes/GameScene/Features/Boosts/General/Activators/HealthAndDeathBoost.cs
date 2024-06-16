using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class HealthAndDeathBoost : IConcreteBoostActivator
    {
        private readonly IHealthContainer _healthContainer;
        private readonly IntBoostData _boostDataProvider;

        public HealthAndDeathBoost(IHealthContainer healthContainer, IntBoostData boostDataProvider)
        {
            _healthContainer = healthContainer;
            _boostDataProvider = boostDataProvider;
        }
        
        public bool IsTimeableBoost => false;
        
        public void Activate()
        {
            _healthContainer.UpdateHealth(_boostDataProvider.Value, false);
        }

        public void Deactivate()
        {
            
        }
    }
}