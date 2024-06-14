using App.Scripts.General.Infrastructure;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces
{
    public interface IBirdsService : IGeneralRestartable
    {
        void AddBird(BirdView birdView);
        void EnableView(BirdView birdView);
        void StopAll();
    }
}