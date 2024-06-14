using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.Extensions.ListExtensions;
using App.Scripts.General.Providers;
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
        private readonly SpriteProvider _spriteProvider;

        public EntityViewService(
            OnTopSprites.Factory spritesFactory,
            SpriteProvider spriteProvider)
        {
            _spritesFactory = spritesFactory;
            _spriteProvider = spriteProvider;
        }

        public void AddBoostSprite(IEntityView entityView, GridItemData itemData, string boostTypeId)
        {
            Sprite boostSprite = _spriteProvider.Sprites[boostTypeId + "_icon"];
            
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

        public void FastAddSprites(IEntityView entityView, EntityStage entityStage, int targetHealth, GridItemData gridItemData)
        {
            List<HealthSpriteData> healthSpriteData = entityStage.AddSpritesOnMainByHp.Where(x => x.Healthes <= targetHealth).ToList();

            foreach (HealthSpriteData data in healthSpriteData)
            {
                OnTopSprites topSprite = _spritesFactory.Create(entityView);
                topSprite.SetSprite(data.Sprites.GetRandomValue());

                gridItemData.Sprites.Add(topSprite);
            }
        }
    }
}