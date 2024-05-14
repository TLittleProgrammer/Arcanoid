using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PositionChecker
{
    public interface IBallPositionChecker : IPositionChecker
    {
        public CollisionTypeId CurrentCollisionTypeId { get; }
        bool CanChangePositionTo(Vector2 targetPosition, ref Vector3 direction);
    }
}