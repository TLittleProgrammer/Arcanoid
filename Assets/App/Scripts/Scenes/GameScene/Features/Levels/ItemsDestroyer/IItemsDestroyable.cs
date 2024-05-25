using System.Collections.Generic;
using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer
{
    public interface IItemsDestroyable : IAsyncInitializable<IEnumerable<DestroyServiceData>>
    {
        void Destroy(GridItemData gridItemData, IEntityView entityView);
    }
}