using System;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces
{
    public interface IBirdPositionChecker : ITickable
    {
        BirdView BirdView { get; }
        event Action<BirdView> BirdFlewAway;
    }
}