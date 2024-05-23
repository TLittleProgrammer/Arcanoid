using App.Scripts.Scenes.GameScene.Levels.ItemsDestroyer;
using App.Scripts.Scenes.GameScene.Levels.ItemsDestroyer.DestroyServices;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class ItemsDestroyableInstaller : Installer<ItemsDestroyableInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<BombDestroyService>().AsSingle();

            Container.Bind<IItemsDestroyable>().To<ItemsDestroyer>().AsSingle();
        }
    }
}