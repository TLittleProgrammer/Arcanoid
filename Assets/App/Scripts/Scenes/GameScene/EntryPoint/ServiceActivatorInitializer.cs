using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.MiniGun;
using App.Scripts.Scenes.GameScene.Features.ServiceActivator;
using App.Scripts.Scenes.GameScene.States;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint
{
    public class ServiceActivatorInitializer : IInitializable
    {
        private readonly IBoostMoveService _boostMoveService;
        private readonly IBoostContainer _boostContainer;
        private readonly IMiniGunService _miniGunService;
        private readonly IServicesActivator _servicesActivator;
        private readonly GameLoopState _gameLoopState;

        public ServiceActivatorInitializer(
            IBoostMoveService boostMoveService,
            IBoostContainer boostContainer,
            IMiniGunService miniGunService,
            IServicesActivator servicesActivator,
            GameLoopState gameLoopState)
        {
            _boostMoveService = boostMoveService;
            _boostContainer = boostContainer;
            _miniGunService = miniGunService;
            _servicesActivator = servicesActivator;
            _gameLoopState = gameLoopState;
        }
        
        public void Initialize()
        {
            _servicesActivator.AddActivable(_boostContainer);
            _servicesActivator.AddActivable(_boostMoveService);
            _servicesActivator.AddActivable(_miniGunService);
            _servicesActivator.AddActivable(_gameLoopState);
        }
    }
}