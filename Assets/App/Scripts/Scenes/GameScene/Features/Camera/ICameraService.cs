using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Camera
{
    public interface ICameraService
    {
        Vector2 ScreenToWorldPoint(Vector2 screenPoint);
    }
}