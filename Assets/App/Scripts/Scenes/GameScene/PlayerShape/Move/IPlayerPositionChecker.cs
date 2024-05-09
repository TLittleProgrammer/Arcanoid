using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PlayerShape.Move
{
    public interface IPlayerPositionChecker
    {
        bool CanChangePositionTo(Vector2 targetPosition);
    }
}