using System;
using App.Scripts.Scenes.GameScene.Features.Components;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.PositionChecker
{
    public interface IPositionChecker : ITickable
    {
        IPositionable Positionable { get; }
        event Action<IPositionable, IPositionChecker> WentAbroad;
    }
}