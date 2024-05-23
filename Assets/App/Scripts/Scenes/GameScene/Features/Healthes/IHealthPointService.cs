﻿using App.Scripts.External.Initialization;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Levels;

namespace App.Scripts.Scenes.GameScene.Features.Healthes
{
    public interface IHealthPointService : IAsyncInitializable<LevelData>, IRestartable
    {
        void UpdateHealth(int healthCount);
    }
}