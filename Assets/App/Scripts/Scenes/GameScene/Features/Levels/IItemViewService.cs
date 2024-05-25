using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;

namespace App.Scripts.Scenes.GameScene.Features.Levels
{
    public interface IItemViewService
    {
        void AddBoostSprite(IEntityView entityView, GridItemData itemData, BoostTypeId boostTypeId);
        void TryAddOnTopSprite(IEntityView entityView, EntityStage entityStage, GridItemData itemData);

        void TryAddOnTopSprite(IEntityView entityView, EntityStage entityStage, GridItemData itemData, int targetHealth);
    }
}