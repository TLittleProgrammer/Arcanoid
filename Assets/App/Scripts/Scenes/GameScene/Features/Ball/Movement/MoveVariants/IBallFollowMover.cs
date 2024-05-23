using App.Scripts.External.Initialization;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public interface IBallFollowMover : IBallMover, IAsyncInitializable
    {
        void Tick();
    }
}