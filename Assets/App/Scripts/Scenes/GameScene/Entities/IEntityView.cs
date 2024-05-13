﻿using App.Scripts.Scenes.GameScene.Components;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Entities
{
    public interface IEntityView : IPositionable, IScalable<Vector3>, IBoxColliderable2D, IGridPositinable, IGameObjectable
    {
        Sprite MainSprite { get; set; }
        Sprite OnTopSprite { get; set; }

        public class Factory : PlaceholderFactory<string, IEntityView>
        {
            
        }
    }
}