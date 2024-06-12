using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Systems;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices.BombDestroyers;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.ServiceActivator;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class EntityDestroyableInstaller : Installer<EntityDestroyableInstaller>
    {

        public override void InstallBindings()
        {
            Container.Bind<IAnimatedDestroyService>().To<AnimatedDestroyService>().AsSingle();
            Container.Bind<IAddVisualDamage>().To<AddVisualDamageService>().AsSingle();
            Container.Bind<SimpleDestroyService>().AsSingle();

            BoostsBind();
            
            Container.BindInterfacesAndSelfTo<CaptiveDestroyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BombDestroyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoostBlockDestroyer>().AsSingle();
            Container.BindInterfacesAndSelfTo<DirectionBombDestroyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ChainDestroyer>().AsSingle();

            Container.Bind<IEntityDestroyable>().To<EntityDestroyer>().AsSingle();
        }

        private void BoostsBind()
        {
            Container.BindInterfacesAndSelfTo<BoostMoveService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoostPositionCheckerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoostContainer>().AsSingle();
            Container.BindInterfacesAndSelfTo<FireballSystem>().AsSingle().WhenNotInjectedInto<ServiceActivator>();
            Container.BindInterfacesAndSelfTo<AutopilotSystem>().AsSingle().WhenNotInjectedInto<ServiceActivator>();

            Container.BindInterfacesAndSelfTo<BoostsActivator>().AsSingle();
        }
    }
}