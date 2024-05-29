using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices.BombDestroyers;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
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

            Container.Bind<BallMoverBoostActivator>().AsSingle();
            Container.Bind<PlayerShapeBoostSize>().AsSingle();
            Container.Bind<ShapeBoostSpeed>().AsSingle();
            Container.Bind<HealthAndDeathBoost>().AsSingle();
            Container.Bind(typeof(ITickable), typeof(AutopilotBoostActivator)).To<AutopilotBoostActivator>().AsSingle();
            Container.Bind<StickyBoostActivator>().AsSingle();
            Container.BindInterfacesAndSelfTo<FireballBoostActivator>().AsSingle();
            Container.BindInterfacesAndSelfTo<MiniGunBoostActivator>().AsSingle();

            Container.BindInterfacesAndSelfTo<BoostsActivator>().AsSingle();
        }
    }
}