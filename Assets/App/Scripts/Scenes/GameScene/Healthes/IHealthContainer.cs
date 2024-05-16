using System;
using System.Collections.Generic;
using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Infrastructure;
using App.Scripts.Scenes.GameScene.Levels;

namespace App.Scripts.Scenes.GameScene.Healthes
{
    public interface IHealthContainer : IAsyncInitializable<LevelData, IEnumerable<IRestartable>>, IRestartable
    {
        event Action LivesAreWasted;
        void UpdateHealth(int healthCount);
    }
}