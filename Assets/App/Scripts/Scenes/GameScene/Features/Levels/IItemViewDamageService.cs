using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Levels.Data;

namespace App.Scripts.Scenes.GameScene.Levels
{
    public interface IItemViewDamageService
    {
        void TryAddOnTopSprite(IEntityView entityView, EntityStage entityStage, GridItemData itemData);

        void TryAddOnTopSprite(IEntityView entityView, EntityStage entityStage, GridItemData itemData, int targetHealth);
    }
}