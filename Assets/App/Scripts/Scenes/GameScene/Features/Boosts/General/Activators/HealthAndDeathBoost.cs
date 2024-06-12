using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Settings;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class HealthAndDeathBoost : IConcreteBoostActivator
    {
        private IHealthContainer _healthContainer;

        public int AddHealthCount;

        public HealthAndDeathBoost(IHealthContainer healthContainer)
        {
            _healthContainer = healthContainer;
        }
        
        public bool IsTimeableBoost => false;
        
        public void Activate()
        {
            _healthContainer.UpdateHealth(AddHealthCount, false);
        }

        public void Deactivate()
        {
            
        }
    }
}