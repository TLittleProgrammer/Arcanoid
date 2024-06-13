using System;
using App.Scripts.External.Components;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Components;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View
{
    public interface IEntityView : IPositionable, IScalable<Vector3>, IBoxColliderable2D, IGameObjectable
    {
        Sprite MainSprite { get; set; }
        Sprite OnTopSprite { get; set; }
        string BoostTypeId { get; set; }
        int EntityId { get; set; }
        public int GridPositionX { get; set; }
        public int GridPositionY { get; set; }
        event Action<IEntityView, Collider2D> Colliderable;

        public class Factory : PlaceholderFactory<string, IEntityView>
        {
            
        }
    }
}