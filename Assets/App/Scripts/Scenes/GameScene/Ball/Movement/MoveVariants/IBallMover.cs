using App.Scripts.General.Infrastructure;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public interface IBallMover : IRestartable
    {
        void UpdateSpeed(float addValue);
    }
}