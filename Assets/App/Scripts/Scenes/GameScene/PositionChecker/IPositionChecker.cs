using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PositionChecker
{
    public interface IPositionChecker
    {
        bool CanChangePositionTo(Vector2 targetPosition);
    }
}