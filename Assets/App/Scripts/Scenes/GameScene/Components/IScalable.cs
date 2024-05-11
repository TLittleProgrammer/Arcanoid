namespace App.Scripts.Scenes.GameScene.Components
{
    public interface IScalable<TScale> : IComponent
    {
        TScale Scale { get; set; }
    }
}