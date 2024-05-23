using App.Scripts.External.Components;

namespace App.Scripts.Scenes.GameScene.Features.Components
{
    public interface IGridPositinable : IComponent
    {
        int GridPositionX { get; set; }
        int GridPositionY { get; set; }
    }
}