using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices
{
    public interface IBlockDestroyService
    {
        void Destroy(GridItemData gridItemData, IEntityView entityView);
    }
}