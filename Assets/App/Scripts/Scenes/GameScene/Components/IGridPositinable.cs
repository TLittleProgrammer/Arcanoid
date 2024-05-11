namespace App.Scripts.Scenes.GameScene.Components
{
    public interface IGridPositinable : IComponent
    {
        int GridPositionX { get; set; }
        int GridPositionY { get; set; }
    }
}