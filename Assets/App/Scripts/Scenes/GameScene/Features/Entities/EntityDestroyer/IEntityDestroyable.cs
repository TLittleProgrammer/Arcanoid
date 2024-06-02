using System.Collections.Generic;
using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer
{
    public interface IEntityDestroyable : IAsyncInitializable<List<DestroyServiceData>>
    {
        void Destroy(GridItemData gridItemData, IEntityView entityView);
    }
}