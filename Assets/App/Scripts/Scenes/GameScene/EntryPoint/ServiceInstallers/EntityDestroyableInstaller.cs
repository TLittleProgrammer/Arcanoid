using System.Collections.Generic;
using System.Linq;
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
    public class EntityDestroyableInstaller : Installer<EntityDestroySettings, EntityDestroyableInstaller>
    {
        private readonly EntityDestroySettings _entityDestroySettings;

        public EntityDestroyableInstaller(EntityDestroySettings entityDestroySettings)
        {
            _entityDestroySettings = entityDestroySettings;
        }
        
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
            
            Container.Bind<Dictionary<string, IBlockDestroyService>>().FromMethod(CreateBoostsDictionary).AsSingle();


            Container.Bind<IEntityDestroyable>().To<EntityDestroyer>().AsSingle();
        }

        private void BoostsBind()
        {
            Container.BindInterfacesAndSelfTo<BoostMoveService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoostPositionCheckerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoostContainer>().AsSingle();
            Container.BindInterfacesAndSelfTo<FireballSystem>().AsSingle().WhenNotInjectedInto<ServiceActivator>();
            Container.BindInterfacesAndSelfTo<AutopilotSystem>().AsSingle().WhenNotInjectedInto<ServiceActivator>();

            Container.Bind<IConcreteBoostActivator>().To<AutopilotBoostActivator>().AsSingle();
            Container.Bind<IConcreteBoostActivator>().To<BallMoverBoostActivator>().AsSingle();
            Container.Bind<IConcreteBoostActivator>().To<FireballBoostActivator>().AsSingle();
            Container.Bind<IConcreteBoostActivator>().To<HealthAndDeathBoost>().AsSingle();
            Container.Bind<IConcreteBoostActivator>().To<MiniGunBoostActivator>().AsSingle();
            Container.Bind<IConcreteBoostActivator>().To<PlayerShapeBoostSize>().AsSingle();
            Container.Bind<IConcreteBoostActivator>().To<ShapeBoostSpeed>().AsSingle();
            Container.Bind<IConcreteBoostActivator>().To<StickyBoostActivator>().AsSingle();

            Container.BindInterfacesAndSelfTo<BoostsActivator>().AsSingle();
        }

        private Dictionary<string, IBlockDestroyService> CreateBoostsDictionary()
        {
            Dictionary<string, IBlockDestroyService> result = new();

            List<IBlockDestroyService> blockDestroyServices = Container.ResolveAll<IBlockDestroyService>().ToList();

            foreach (DestroySettingsServiceData data in _entityDestroySettings.DestroyServiceDatas)
            {
                foreach (IBlockDestroyService destroyService in blockDestroyServices)
                {
                    if (data.BlockDestroyService.GetType() == destroyService.GetType())
                    {
                        foreach (string id in data.DestroyingIds)
                        {
                            result.Add(id, destroyService);
                        }
                    }
                }
            }
            
            return result;
        }
    }
}