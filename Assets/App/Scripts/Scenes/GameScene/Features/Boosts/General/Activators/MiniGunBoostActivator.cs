using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Spawn;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public class MiniGunBoostActivator : IConcreteBoostActivator
    {
        private readonly IMiniGunService _miniGunService;

        public MiniGunBoostActivator(IMiniGunService miniGunService)
        {
            _miniGunService = miniGunService;
        }
        
        public bool IsTimeableBoost => true;
        
        public void Activate(IBoostDataProvider boostDataProvider)
        {
            _miniGunService.ActiveMiniGun = true;
        }

        public void Deactivate()
        {
            _miniGunService.ActiveMiniGun = false;
        }
    }
}