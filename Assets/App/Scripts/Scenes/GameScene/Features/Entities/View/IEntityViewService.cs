using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Entities.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Grid;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View
{
    public interface IEntityViewService
    {
        void AddBoostSprite(IEntityView entityView, GridItemData itemData, BoostTypeId boostTypeId);
        void TryAddOnTopSprite(IEntityView entityView, EntityStage entityStage, GridItemData itemData);

        void TryAddOnTopSprite(IEntityView entityView, EntityStage entityStage, GridItemData itemData, int targetHealth);
        void FastAddSprites(IEntityView entityView, EntityStage entityStage, int targetHealth, GridItemData gridItemData);
    }
}