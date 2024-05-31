using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Entities.TopSprites;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.OnTopSprite
{
    public class OnTopSpriteFactory : IFactory<IEntityView, OnTopSprites>
    {
        private readonly OnTopSprites.Pool _onTopSprites;

        public OnTopSpriteFactory(OnTopSprites.Pool onTopSprites)
        {
            _onTopSprites = onTopSprites;
        }
        
        public OnTopSprites Create(IEntityView targetType)
        {
            OnTopSprites sprite = _onTopSprites.Spawn();

            Transform spriteTransform     = sprite.transform;
            spriteTransform.parent        = targetType.GameObject.transform;
            spriteTransform.localPosition = Vector3.zero;
            spriteTransform.localScale    = Vector3.one;

            return sprite;
        }
    }
}