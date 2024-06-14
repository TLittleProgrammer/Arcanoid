using App.Scripts.Scenes.GameScene.Features.Entities.Bird;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.PositionSystems;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint
{
    public class BirdsInstaller : Installer<BirdsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BirdPositionCheckerSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<BirdMovementContainerSystem>().AsSingle();
        }
    }
}