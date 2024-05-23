using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Pools;
using App.Scripts.Scenes.GameScene.Features.TopSprites;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.OnTopSprite
{
    public class OnTopSpriteFactory : IFactory<IEntityView, OnTopSprites>
    {
        private readonly IPoolContainer _poolContainer;

        public OnTopSpriteFactory(IPoolContainer poolContainer)
        {
            _poolContainer = poolContainer;
        }
        
        public OnTopSprites Create(IEntityView entityView)
        {
            OnTopSprites sprite = _poolContainer.GetItem<OnTopSprites>(PoolTypeId.OnTopSprite);

            Transform spriteTransform     = sprite.transform;
            spriteTransform.parent        = entityView.GameObject.transform;
            spriteTransform.localPosition = Vector3.zero;
            spriteTransform.localScale    = Vector3.one;

            return sprite;
        }
    }
}