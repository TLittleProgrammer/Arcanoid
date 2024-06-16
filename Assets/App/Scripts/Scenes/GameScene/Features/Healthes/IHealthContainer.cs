using System;
using App.Scripts.External.Initialization;
using App.Scripts.General.Infrastructure;

namespace App.Scripts.Scenes.GameScene.Features.Healthes
{
    public interface IHealthContainer : IAsyncInitializable<int>, IGeneralRestartable
    {
        event Action LivesAreWasted;
        event Action<int> GetDamage;
        void UpdateHealth(int healthCount, bool needRestart = true);
    }
}