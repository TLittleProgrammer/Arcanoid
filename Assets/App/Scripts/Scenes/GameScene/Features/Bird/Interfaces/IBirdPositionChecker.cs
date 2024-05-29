using System;
using App.Scripts.External.Components;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Bird
{
    public interface IBirdPositionChecker : ITickable
    {
        BirdView BirdView { get; }
        event Action<BirdView, IBirdPositionChecker> BirdFlewAway;
    }
}