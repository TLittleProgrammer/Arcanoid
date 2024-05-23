using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PositionChecker
{
    public interface IShapePositionChecker : IPositionChecker
    {
        bool CanChangePositionTo(Vector2 targetPosition);
    }
}