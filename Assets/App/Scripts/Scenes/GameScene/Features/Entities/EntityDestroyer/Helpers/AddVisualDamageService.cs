using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers
{
    public sealed class AddVisualDamageService : IAddVisualDamage
    {
        private readonly IEntityViewService _entityViewService;
        private readonly ILevelViewUpdater _levelViewUpdater;

        public AddVisualDamageService(IEntityViewService entityViewService, ILevelViewUpdater levelViewUpdater)
        {
            _entityViewService = entityViewService;
            _levelViewUpdater = levelViewUpdater;
        }
        
        public void AddVisualDamage(int damage, GridItemData gridItemData, IEntityView entityView)
        {
            int currentHealth = gridItemData.CurrentHealth;

            gridItemData.CurrentHealth -= damage;
            EntityStage entityStage = _levelViewUpdater.GetEntityStage(entityView);

            for (int i = currentHealth; i >= gridItemData.CurrentHealth; i--)
            {
                _entityViewService.TryAddOnTopSprite(entityView, entityStage, gridItemData, i);
            }
        }
    }
}