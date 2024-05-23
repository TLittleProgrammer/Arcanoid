using UnityEngine;

namespace App.Scripts.Scenes.GameScene.PlayerShape
{
    public interface IRectMousePositionChecker
    {
        bool MouseOnRect(Vector2 mousePosition);
    }
}