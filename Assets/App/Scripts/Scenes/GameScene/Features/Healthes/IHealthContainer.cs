using System;
using System.Collections.Generic;
using App.Scripts.External.Initialization;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Levels;

namespace App.Scripts.Scenes.GameScene.Features.Healthes
{
    public interface IHealthContainer : IAsyncInitializable<LevelData, List<IRestartable>>, IRestartable
    {
        event Action LivesAreWasted;
        void UpdateHealth(int healthCount, bool needRestart = true);
    }
}