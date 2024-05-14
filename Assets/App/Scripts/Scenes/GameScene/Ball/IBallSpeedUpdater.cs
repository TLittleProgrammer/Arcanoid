using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Ball.Movement;

namespace App.Scripts.Scenes.GameScene.Ball
{
    public interface IBallSpeedUpdater : IAsyncInitializable<IBallMovementService>
    {
        void UpdateSpeed();
    }
}