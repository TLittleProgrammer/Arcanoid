using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Load;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class LevelServicesInstaller : Installer<LevelServicesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelViewUpdater>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelLoader>().AsSingle();
        }
    }
}