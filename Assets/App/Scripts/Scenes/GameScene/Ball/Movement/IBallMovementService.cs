using App.Scripts.Scenes.GameScene.Infrastructure;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Ball.Movement
{
    public interface IBallMovementService : ITickable, IRestartable
    {
        
    }
}