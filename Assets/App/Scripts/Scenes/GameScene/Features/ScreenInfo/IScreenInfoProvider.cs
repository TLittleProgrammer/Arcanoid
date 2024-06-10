using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.ScreenInfo
{
    public interface IScreenInfoProvider
    {
        float WidthInPixels { get; }
        float HeightInPixels { get; }
        float WidthInWorld { get; }
        float HeightInWorld { get; }
        
        Vector2 ScreenWorldSize { get; }
    }
}