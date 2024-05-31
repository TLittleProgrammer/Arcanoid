using System;
using App.Scripts.General.Infrastructure;

namespace App.Scripts.Scenes.GameScene.Features.Time
{
    public interface ITimeProvider : IGeneralRestartable
    {
        float DeltaTime { get; }
        float TimeScale { get; set;  }
        event Action TimeScaleChanged;
    }
}