namespace App.Scripts.Scenes.GameScene.ScreenInfo
{
    public interface IScreenInfoProvider
    {
        float WidthInPixels { get; }
        float HeightInPixels { get; }
        float WidthInWorld { get; }
        float HeightInWorld { get; }
    }
}