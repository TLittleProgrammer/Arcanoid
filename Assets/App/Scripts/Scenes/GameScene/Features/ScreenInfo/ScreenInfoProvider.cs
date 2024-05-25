using App.Scripts.Scenes.GameScene.Features.Camera;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ScreenInfo
{
    public class ScreenInfoProvider : IScreenInfoProvider
    {
        public float WidthInPixels { get; }
        public float HeightInPixels { get; }
        public float WidthInWorld  { get; private set; }
        public float HeightInWorld { get; private set; }

        public ScreenInfoProvider(ICameraService cameraService)
        {
            WidthInPixels  = Screen.width;
            HeightInPixels = Screen.height;

            CalculateWorldSize(cameraService);
        }

        private void CalculateWorldSize(ICameraService cameraService)
        {
            WidthInWorld = cameraService.ScreenToWorldPoint(new Vector2(WidthInPixels, 0f)).x * 2;
            HeightInWorld = cameraService.ScreenToWorldPoint(new Vector2(0f, HeightInPixels)).y * 2;
        }
    }
}