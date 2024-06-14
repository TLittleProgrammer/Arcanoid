using App.Scripts.Scenes.GameScene.Features.Components;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird
{
    public interface IBirdMovementContainerSystem : ITickable, IActivable
    {
        void AddBird(BirdView birdView);
        void RemoveBird(BirdView birdView);
        void StopAll();
        void Restart();
    }
}