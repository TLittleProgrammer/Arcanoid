using App.Scripts.General.Infrastructure;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement
{
    public interface IBallMovementService : ITickable, IRestartable
    {
        bool IsFreeFlight { get; }
        Vector2 Velocity { get; }
        void UpdateSpeed(float progress);
        void GoFly();
        void Sticky();
        void SetSpeedMultiplier(float speedMultiplier);
        void SetVelocity(Vector2 velocity);
    }
}