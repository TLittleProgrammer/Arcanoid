using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.MiniGun;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Helpers
{
    public class MiniGunBoostActivator : IConcreteBoostActivator
    {
        private readonly IMiniGunService _miniGunService;

        public MiniGunBoostActivator(IBoostContainer boostContainer, IMiniGunService miniGunService)
        {
            _miniGunService = miniGunService;
            boostContainer.BoostEnded += OnBoostEnded;
        }

        public void Activate(BoostTypeId boostTypeId)
        {
            _miniGunService.ActiveMiniGun = true;
        }

        private void OnBoostEnded(BoostTypeId boostType)
        {
            if (boostType is BoostTypeId.MiniGun)
            {
                _miniGunService.ActiveMiniGun = false;
            }
        }
    }
}