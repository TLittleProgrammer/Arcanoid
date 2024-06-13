using App.Scripts.External.Components;
using App.Scripts.Scenes.GameScene.Features.Components;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces
{
    public interface IBoostView : ITransformable, ISpriteRenderable
    {
        public string BoostTypeId { get; set; }
    }
}