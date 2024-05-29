using App.Scripts.External.Initialization;
using App.Scripts.General.Infrastructure;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants
{
    public interface IBallFreeFlightMover : IBallMover, IAsyncInitializable<Vector2>, IRestartable
    {
        float ConstantSpeed { get; }
        void SetSpeed(float targetValue);
        void Reset();
        void Continue();
        void SetSpeedMultiplier(float speedMultiplier);
    }
}