using System;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces
{
    public interface IBirdHealthPointContainer
    {
        event Action<BirdView> BirdDied;
        void AddBird(BirdView birdView);
    }
}