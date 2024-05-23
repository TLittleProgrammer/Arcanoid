using System.Collections.Generic;
using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer
{
    public interface IItemsDestroyable : IAsyncInitializable<IEnumerable<DestroyServiceData>>
    {
        void Destroy(GridItemData gridItemData, IEntityView entityView);
    }
}