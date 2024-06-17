using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.PositionChecker;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.PositionCheckers
{
    public interface IPositionCheckerSystem : ITickable
    {
        void Add(IPositionChecker positionChecker);
        void Remove(IPositionChecker positionChecker);
        void RemoveAllByPositionable(List<IPositionable> balls);
    }
}