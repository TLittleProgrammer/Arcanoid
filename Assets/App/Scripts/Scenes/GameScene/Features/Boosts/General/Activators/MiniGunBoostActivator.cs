using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Spawn;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public class MiniGunBoostActivator : IConcreteBoostActivator
    {
        private readonly IMiniGunService _miniGunService;

        public MiniGunBoostActivator(IMiniGunService miniGunService)
        {
            _miniGunService = miniGunService;
        }

        public void Activate(BoostTypeId boostTypeId)
        {
            _miniGunService.ActiveMiniGun = true;
        }

        public void Deactivate()
        {
            _miniGunService.ActiveMiniGun = false;
        }
    }
}