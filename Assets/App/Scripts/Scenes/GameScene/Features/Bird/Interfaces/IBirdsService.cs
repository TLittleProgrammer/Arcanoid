using System.Collections.Generic;
using App.Scripts.General.Infrastructure;

namespace App.Scripts.Scenes.GameScene.Features.Bird
{
    public interface IBirdsService : IRestartable
    {
        Dictionary<BirdView, IBirdMovement> Birds { get; }
        void AddBird(BirdView birdView);
        void GoFly(BirdView birdView);
        void Destroy(BirdView birdView);
    }
}