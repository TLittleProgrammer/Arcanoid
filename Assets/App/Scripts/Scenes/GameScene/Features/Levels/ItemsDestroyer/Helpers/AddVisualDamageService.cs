using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.View;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers
{
    public sealed class AddVisualDamageService : IAddVisualDamage
    {
        private readonly IItemViewService _itemViewService;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly CircleEffect.Factory _circleEffectFactory;

        public AddVisualDamageService(IItemViewService itemViewService, ILevelViewUpdater levelViewUpdater, CircleEffect.Factory circleEffectFactory)
        {
            _itemViewService = itemViewService;
            _levelViewUpdater = levelViewUpdater;
            _circleEffectFactory = circleEffectFactory;
        }
        
        public void AddVisualDamage(int damage, GridItemData gridItemData, IEntityView entityView)
        {
            int currentHealth = gridItemData.CurrentHealth;

            gridItemData.CurrentHealth -= damage;
            EntityStage entityStage = _levelViewUpdater.GetEntityStage(entityView);

            for (int i = currentHealth; i >= gridItemData.CurrentHealth; i--)
            {
                _itemViewService.TryAddOnTopSprite(entityView, entityStage, gridItemData, i);
            }

            CircleEffect circleEffect = _circleEffectFactory.Create(entityView as EntityView);
            circleEffect.PlayEffect();
        }
    }
}