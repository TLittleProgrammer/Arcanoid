using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Camera
{
    public interface ICameraService
    {
        UnityEngine.Camera Camera { get; }
        Vector2 ScreenToWorldPoint(Vector2 screenPoint);
    }
}