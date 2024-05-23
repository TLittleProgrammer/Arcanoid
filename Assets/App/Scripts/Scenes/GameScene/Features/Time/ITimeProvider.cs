using System;
using App.Scripts.General.Infrastructure;

namespace App.Scripts.Scenes.GameScene.Time
{
    public interface ITimeProvider : IRestartable
    {
        float DeltaTime { get; }
        float TimeScale { get; set;  }
        event Action TimeScaleChanged;
    }
}