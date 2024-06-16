using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird.Factory
{
    public interface IBirdMovementSystemFactory
    {
        IBirdMovement Create(BirdView birdView);
    }
}