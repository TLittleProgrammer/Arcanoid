using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Camera
{
    public interface ICameraService
    {
        Vector2 ScreenToWorldPoint(Vector2 screenPoint);
    }
}