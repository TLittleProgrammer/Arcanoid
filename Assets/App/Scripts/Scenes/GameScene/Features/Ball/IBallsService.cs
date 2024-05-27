using App.Scripts.Scenes.GameScene.Features.Components;

namespace App.Scripts.Scenes.GameScene.Features.Ball
{
    public interface IBallsService
    {
        void AddBall(IPositionable positionable);
    }
}