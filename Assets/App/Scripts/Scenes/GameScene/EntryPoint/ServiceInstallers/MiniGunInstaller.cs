using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class MiniGunInstaller : Installer<MiniGunInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BulletPositionChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletMovement>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletCollisionService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SpawnBulletService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletCollisionHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MiniGunService>().AsSingle();
        }
    }
}