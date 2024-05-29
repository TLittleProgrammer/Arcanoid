using App.Scripts.External.Initialization;
using App.Scripts.General.Infrastructure;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants
{
    public interface IBallFollowMover : IBallMover, IAsyncInitializable, IRestartable
    {
        void Tick();
    }
}