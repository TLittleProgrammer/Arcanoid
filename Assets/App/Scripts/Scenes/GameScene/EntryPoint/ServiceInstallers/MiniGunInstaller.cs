using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Collision;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Movement;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Spawn;
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