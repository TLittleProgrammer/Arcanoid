using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PositionChecker
{
    public interface IBallPositionChecker : IPositionChecker
    {
        bool CanChangePositionTo(Vector2 targetPosition, ref Vector3 direction);
    }
}