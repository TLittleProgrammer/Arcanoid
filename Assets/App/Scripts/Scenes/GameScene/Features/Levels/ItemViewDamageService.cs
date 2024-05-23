using System.Linq;
using App.Scripts.External.Extensions.ListExtensions;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.TopSprites;

namespace App.Scripts.Scenes.GameScene.Features.Levels
{
    public class ItemViewDamageService : IItemViewDamageService
    {
        private readonly OnTopSprites.Factory _spritesFactory;

        public ItemViewDamageService(OnTopSprites.Factory spritesFactory)
        {
            _spritesFactory = spritesFactory;
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