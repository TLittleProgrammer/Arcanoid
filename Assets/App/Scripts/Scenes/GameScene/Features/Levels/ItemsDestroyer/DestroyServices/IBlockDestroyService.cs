using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Levels.Data;

namespace App.Scripts.Scenes.GameScene.Levels.ItemsDestroyer.DestroyServices
{
    public interface IBlockDestroyService
    {
        void Destroy(GridItemData gridItemData, IEntityView entityView);
    }
}