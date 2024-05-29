using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun;
using App.Scripts.Scenes.GameScene.Features.Entities;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
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