using App.Scripts.General.Infrastructure;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Ball.Movement
{
    public interface IBallMovementService : ITickable, IRestartable
    {
        bool IsFreeFlight { get; set; }
        void UpdateSpeed(float addValue);
        void GoFly();
        void Sticky();
        void SetSpeedMultiplier(float speedMultiplier);
    }
}