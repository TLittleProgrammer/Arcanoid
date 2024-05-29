using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Entities.TopSprites;
using App.Scripts.Scenes.GameScene.Features.Pools;
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
        
        public OnTopSprites Create(IEntityView targetType)
        {
            OnTopSprites sprite = _poolContainer.GetItem<OnTopSprites>(PoolTypeId.OnTopSprite);

            Transform spriteTransform     = sprite.transform;
            spriteTransform.parent        = targetType.GameObject.transform;
            spriteTransform.localPosition = Vector3.zero;
            spriteTransform.localScale    = Vector3.one;

            return sprite;
        }
    }
}