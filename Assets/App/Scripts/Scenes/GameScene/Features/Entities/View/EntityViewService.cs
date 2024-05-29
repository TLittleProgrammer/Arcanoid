using System.Linq;
using App.Scripts.External.Extensions.ListExtensions;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Entities.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Entities.TopSprites;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View
{
    public class EntityViewService : IEntityViewService
    {
        private readonly OnTopSprites.Factory _spritesFactory;
        private readonly BoostViewProvider _boostViewProvider;

        public EntityViewService(
            OnTopSprites.Factory spritesFactory,
            BoostViewProvider boostViewProvider)
        {
            _spritesFactory = spritesFactory;
            _boostViewProvider = boostViewProvider;
        }

        public void AddBoostSprite(IEntityView entityView, GridItemData itemData, BoostTypeId boostTypeId)
        {
            Sprite boostSprite = _boostViewProvider.Icons[boostTypeId];
            
            OnTopSprites topSprite = _spritesFactory.Create(entityView);
            topSprite.SetSprite(boostSprite);

            itemData.Sprites.Add(topSprite);
        }

        public void TryAddOnTopSprite(IEntityView entityView, EntityStage entityStage, GridItemData itemData)
        {
            TryAddOnTopSprite(entityView, entityStage, itemData, itemData.CurrentHealth);
        }
        
        public void TryAddOnTopSprite(IEntityView entityView, EntityStage entityStage, GridItemData itemData, int targetHealth)
        {
            HealthSpriteData healthSpriteData = entityStage.AddSpritesOnMainByHp.FirstOrDefault(x => x.Healthes == targetHealth);

            if (healthSpriteData is not null)
            {
                OnTopSprites topSprite = _spritesFactory.Create(entityView);
                topSprite.SetSprite(healthSpriteData.Sprites.GetRandomValue());

                itemData.Sprites.Add(topSprite);
            }
        }
    }
}