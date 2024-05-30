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
            Container.Bind(typeof(ILevelViewUpdater), typeof(ILevelProgressSavable)).To<LevelViewUpdater>().AsSingle();
            Container.Bind(typeof(ILevelLoader), typeof(ILevelProgressSavable), typeof(ICurrentLevelRestartable)).To<LevelLoader>().AsSingle();
        }
    }
}