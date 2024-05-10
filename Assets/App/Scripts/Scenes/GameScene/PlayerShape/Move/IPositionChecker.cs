using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PlayerShape.Move
{
    public interface IPositionChecker
    {
        bool CanChangePositionTo(Vector2 targetPosition);
    }
}