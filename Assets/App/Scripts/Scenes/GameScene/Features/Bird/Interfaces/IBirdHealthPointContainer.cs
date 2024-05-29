using System;

namespace App.Scripts.Scenes.GameScene.Features.Bird
{
    public interface IBirdHealthPointContainer
    {
        event Action<BirdView> BirdDied;
        void AddBird(BirdView birdView);
    }
}