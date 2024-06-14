using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird.PositionSystems
{
    public interface IBirdPositionCheckerSystem : ITickable, IActivable
    {
        IBirdPositionChecker AddBird(BirdView birdView);
        void RemoveBird(BirdView birdView);
    }
}