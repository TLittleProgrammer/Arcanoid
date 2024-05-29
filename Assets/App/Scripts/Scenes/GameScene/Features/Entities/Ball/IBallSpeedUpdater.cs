using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball
{
    public interface IBallSpeedUpdater : IAsyncInitializable<IBallMovementService>
    {
        void UpdateSpeed();
    }
}