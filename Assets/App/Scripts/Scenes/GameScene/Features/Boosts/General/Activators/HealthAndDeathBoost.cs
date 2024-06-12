﻿using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Settings;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class HealthAndDeathBoost : IConcreteBoostActivator
    {
        private readonly IHealthContainer _healthContainer;

        private readonly BoostsSettings _boostsSettings;

        public HealthAndDeathBoost(IHealthContainer healthContainer, BoostsSettings boostsSettings)
        {
            _healthContainer = healthContainer;
            _boostsSettings = boostsSettings;
        }
        
        public bool IsTimeableBoost => false;
        
        public void Activate(BoostTypeId boostTypeId)
        {
            if (boostTypeId is BoostTypeId.AddHealth)
            {
                _healthContainer.UpdateHealth(_boostsSettings.AddHealth, false);
            }
            else
            {
                _healthContainer.UpdateHealth(-_boostsSettings.MinusHealth, false);
            }
        }

        public void Deactivate()
        {
            
        }
    }
}