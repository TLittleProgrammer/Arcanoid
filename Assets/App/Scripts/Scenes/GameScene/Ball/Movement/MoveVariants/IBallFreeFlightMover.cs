using App.Scripts.External.Initialization;
using App.Scripts.General.Infrastructure;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public interface IBallFreeFlightMover : IBallMover, IAsyncInitializable<Vector2>, IRestartable
    {
        float VelocitySpeed { get; }
        float GeneralSpeed { get; }
        void UpdateSpeed(float addValue);
    }
}