using App.Scripts.Scenes.GameScene.Components;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Entities
{
    public interface IEntityView : ITransformable, IScalable, IBoxColliderable2D
    {
        Sprite MainSprite { get; set; }
        Sprite OnTopSprite { get; set; }

        public class Factory : PlaceholderFactory<string, IEntityView>
        {
            
        }
    }
}