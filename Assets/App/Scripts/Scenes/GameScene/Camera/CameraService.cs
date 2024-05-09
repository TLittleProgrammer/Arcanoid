using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Camera
{
    public sealed class CameraService : ICameraService
    {
        private readonly UnityEngine.Camera _camera;
        
        public CameraService(UnityEngine.Camera camera)
        {
            _camera = camera;
        }

        public Vector2 ScreenToWorldPoint(Vector2 screenPoint)
        {
            return _camera.ScreenToWorldPoint(screenPoint);
        }
    }
}