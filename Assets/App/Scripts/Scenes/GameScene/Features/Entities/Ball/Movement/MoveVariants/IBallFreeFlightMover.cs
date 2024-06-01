using App.Scripts.External.CustomData;
using App.Scripts.External.Initialization;
using App.Scripts.General.Infrastructure;
using Unity.Mathematics;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants
{
    public interface IBallFreeFlightMover : IBallMover, IAsyncInitializable<Vector2>, IRestartable
    {
        Vector2 Velocity { get; set; }
        void SetSpeed(float targetValue);
        void Reset();
        void SetSpeedMultiplier(float speedMultiplier);
        void SetVelocity(Vector2 velocity);
    }
}