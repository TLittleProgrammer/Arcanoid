using App.Scripts.External.Components;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Entities;

namespace App.Scripts.Scenes.GameScene.Features.Boosts
{
    public interface IBoostView : ITransformable, ISpriteRenderable
    {
        public BoostTypeId BoostTypeId { get; set; }
    }
}