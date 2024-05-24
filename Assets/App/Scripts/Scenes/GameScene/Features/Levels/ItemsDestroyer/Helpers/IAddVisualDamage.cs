using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers
{
    public interface IAddVisualDamage
    {
        void AddVisualDamage(int damage, GridItemData gridItemData, IEntityView entityView);
    }
}