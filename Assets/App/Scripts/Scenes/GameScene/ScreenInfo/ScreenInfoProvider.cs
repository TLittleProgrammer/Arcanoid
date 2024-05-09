using UnityEngine;

namespace App.Scripts.Scenes.GameScene.ScreenInfo
{
    public class ScreenInfoProvider : IScreenInfoProvider
    {
        public float Width { get; }
        public float Height { get; }

        public ScreenInfoProvider()
        {
            Width  = Screen.width;
            Height = Screen.height;
        }
    }
}