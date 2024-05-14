using App.Scripts.Scenes.GameScene.Infrastructure;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public interface IBallMover : ITickable, IRestartable
    {
        void UpdateSpeed(float addValue);
    }
}