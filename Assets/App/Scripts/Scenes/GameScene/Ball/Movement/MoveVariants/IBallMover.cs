using App.Scripts.Scenes.GameScene.Infrastructure;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public interface IBallMover : IRestartable
    {
        void UpdateSpeed(float addValue);
        void Tick();
    }
}