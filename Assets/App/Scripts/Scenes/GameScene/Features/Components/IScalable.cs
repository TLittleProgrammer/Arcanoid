using App.Scripts.External.Components;

namespace App.Scripts.Scenes.GameScene.Features.Components
{
    public interface IScalable<TScale> : IComponent
    {
        TScale Scale { get; set; }
    }
}