using System;
using App.Scripts.Scenes.GameScene.Features.Components;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Ball.PositionChecker
{
    public interface IBallPositionChecker : ITickable
    {
        IPositionable BallPositionable { get; }
        event Action<IPositionable> BallFallen;
    }
}