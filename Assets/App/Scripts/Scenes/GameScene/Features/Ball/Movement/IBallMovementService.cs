using App.Scripts.General.Infrastructure;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Ball.Movement
{
    public interface IBallMovementService : ITickable, IRestartable
    {
        bool IsFreeFlight { get; set; }
        void UpdateSpeed(float progress);
        void GoFly();
        void Sticky();
        void SetSpeedMultiplier(float speedMultiplier);
    }
}