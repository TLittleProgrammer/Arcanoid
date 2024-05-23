﻿using App.Scripts.Scenes.GameScene.Features.Levels.Load;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class LevelServicesInstaller : Installer<LevelServicesInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ILevelViewUpdater>().To<LevelViewUpdater>().AsSingle();
            Container.Bind<ILevelLoader>().To<LevelLoader>().AsSingle();
        }
    }
}