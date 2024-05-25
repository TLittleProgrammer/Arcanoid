using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer
{
    public sealed class ItemsDestroyer : IItemsDestroyable
    {
        private readonly SimpleDestroyService _simpleDestroyService;

        private Dictionary<BoostTypeId,IBlockDestroyService> _destroyServices;

        public ItemsDestroyer(
            SimpleDestroyService simpleDestroyService)
        {
            _simpleDestroyService = simpleDestroyService;
        }
        
        public async UniTask AsyncInitialize(IEnumerable<DestroyServiceData> param)
        {
            _destroyServices = param.ToDictionary(x => x.BoostTypeId, x => x.BlockDestroyService);

            await UniTask.CompletedTask;
        }

        public void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            if (_destroyServices.ContainsKey(gridItemData.BoostTypeId))
            {
                _destroyServices[gridItemData.BoostTypeId].Destroy(gridItemData, entityView);
                return;
            }

            _simpleDestroyService.Destroy(gridItemData, entityView);
        }
    }
}