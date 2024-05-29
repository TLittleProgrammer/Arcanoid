using System.Collections.Generic;

namespace App.Scripts.Scenes.GameScene.Features.Bird
{
    public interface IBirdsService
    {
        Dictionary<BirdView, IBirdMovement> Birds { get; }
        void AddBird(BirdView birdView);
        void GoFly(BirdView birdView);
        void Destroy(BirdView birdView);
    }
}