using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices
{
    public interface IBlockDestroyService
    {
        BoostTypeId[] ProccessingBoostTypes { get; }
        void Destroy(GridItemData gridItemData, IEntityView entityView);
    }
}