﻿using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Infrastructure;
using App.Scripts.Scenes.GameScene.Levels;

namespace App.Scripts.Scenes.GameScene.Healthes
{
    public interface IHealthPointService : IAsyncInitializable<LevelData>, IRestartable
    {
        void UpdateHealth(int healthCount);
    }
}