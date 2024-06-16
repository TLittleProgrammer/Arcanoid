using App.Scripts.Scenes.GameScene.Features.Camera;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ScreenInfo
{
    public class ScreenInfoProvider : IScreenInfoProvider
    {
        private readonly ICameraService _cameraService;
        public float WidthInPixels { get; }
        public float HeightInPixels { get; }
        public float WidthInWorld  { get; private set; }
        public float HeightInWorld { get; private set; }
        public Vector2 ScreenWorldSize { get; private set; }

        public ScreenInfoProvider(ICameraService cameraService)
        {
            _cameraService = cameraService;
            WidthInPixels  = Screen.width;
            HeightInPixels = Screen.height;
            
            Initialize();
        }

        private void Initialize()
        {
            CalculateWorldSize();
            CalculateScreenSize();
        }

        private void CalculateWorldSize()
        {
            WidthInWorld = _cameraService.ScreenToWorldPoint(new Vector2(WidthInPixels, 0f)).x * 2;
            HeightInWorld = _cameraService.ScreenToWorldPoint(new Vector2(0f, HeightInPixels)).y * 2;
        }

        private void CalculateScreenSize()
        {
            Vector2 topRightPoint = _cameraService.ScreenToWorldPoint(new Vector2(WidthInPixels, HeightInPixels));
            topRightPoint *= 2;

            ScreenWorldSize = topRightPoint;
        }
    }
}