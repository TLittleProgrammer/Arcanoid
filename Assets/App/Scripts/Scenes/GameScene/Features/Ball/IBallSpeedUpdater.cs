using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement;

namespace App.Scripts.Scenes.GameScene.Features.Ball
{
    public interface IBallSpeedUpdater : IAsyncInitializable<IBallMovementService>
    {
        void UpdateSpeed();
    }
}