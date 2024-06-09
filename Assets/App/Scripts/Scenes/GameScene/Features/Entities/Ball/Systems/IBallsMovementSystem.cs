using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Systems
{
    public interface IBallsMovementSystem : ITickable
    {
        void AddBall(BallView ballView);
        IBallMovementService GetMovementService(BallView ballView);
    }
}