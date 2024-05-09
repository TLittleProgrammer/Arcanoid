using System;

namespace App.Scripts.Scenes.GameScene.Time
{
    public interface ITimeProvider
    {
        float DeltaTime { get; }
        float TimeScale { get; }
        event Action TimeScaleChanged;
    }
}