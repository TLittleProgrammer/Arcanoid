using App.Scripts.External.Components;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Components;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View
{
    public interface IEntityView : IPositionable, IScalable<Vector3>, IBoxColliderable2D, IGridPositinable, IGameObjectable
    {
        Sprite MainSprite { get; set; }
        Sprite OnTopSprite { get; set; }
        BoostTypeId BoostTypeId { get; set; }
        int EntityId { get; set; }
        
        public class Factory : PlaceholderFactory<string, IEntityView>
        {
            
        }
    }
}