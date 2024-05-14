using System;
using App.Scripts.Scenes.GameScene.Infrastructure;

namespace App.Scripts.Scenes.GameScene.Time
{
    public interface ITimeProvider : IRestartable
    {
        float DeltaTime { get; }
        float TimeScale { get; set;  }
        event Action TimeScaleChanged;
    }
}