using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers
{
    public interface IAddVisualDamage
    {
        void AddVisualDamage(int damage, GridItemData gridItemData, IEntityView entityView);
    }
}