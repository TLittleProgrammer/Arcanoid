using System;
using App.Scripts.Scenes.GameScene.Features.Components;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.PositionChecker
{
    public interface IBallPositionChecker : ITickable
    {
        IPositionable BallPositionable { get; }
        event Action<IPositionable> BallFallen;
    }
}