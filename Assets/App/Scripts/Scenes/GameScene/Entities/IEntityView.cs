using App.Scripts.Scenes.GameScene.Interfaces;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Entities
{
    public interface IEntityView : ITransformable, IScalable
    {
        Sprite MainSprite { get; set; }
        Sprite OnTopSprite { get; set; }

        public class Factory : PlaceholderFactory<string, IEntityView>
        {
            
        }
    }
}