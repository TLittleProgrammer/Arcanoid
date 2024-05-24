using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class ItemsDestroyableInstaller : Installer<ItemsDestroyableInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAnimatedDestroyService>().To<AnimatedDestroyService>().AsSingle();
            Container.Bind<IAddVisualDamage>().To<AddVisualDamageService>().AsSingle();
            Container.Bind<SimpleDestroyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoostMoveService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoostPositionCheckerService>().AsSingle();
            
            Container.Bind<BombDestroyService>().AsSingle();
            Container.Bind<BallSpeedBoostsDestroyer>().AsSingle();

            Container.Bind<IItemsDestroyable>().To<ItemsDestroyer>().AsSingle();
        }
    }
}