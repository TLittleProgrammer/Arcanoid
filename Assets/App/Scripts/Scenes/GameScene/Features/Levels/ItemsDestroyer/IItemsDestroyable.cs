using System.Collections.Generic;
using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Levels.Data;
using App.Scripts.Scenes.GameScene.Levels.ItemsDestroyer.DestroyServices;

namespace App.Scripts.Scenes.GameScene.Levels.ItemsDestroyer
{
    public interface IItemsDestroyable : IAsyncInitializable<IEnumerable<DestroyServiceData>>
    {
        void Destroy(GridItemData gridItemData, IEntityView entityView);
    }
}