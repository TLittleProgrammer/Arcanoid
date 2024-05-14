using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PositionChecker
{
    public interface IShapePositionChecker : IPositionChecker
    {
        bool CanChangePositionTo(Vector2 targetPosition);
    }
}