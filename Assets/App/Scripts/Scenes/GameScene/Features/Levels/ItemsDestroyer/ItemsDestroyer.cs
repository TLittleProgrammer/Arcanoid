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
        private readonly IAnimatedDestroyService _animatedDestroyService;

        private Dictionary<BoostTypeId,IBlockDestroyService> _destroyServices;

        public ItemsDestroyer(
            SimpleDestroyService simpleDestroyService,
            IAnimatedDestroyService animatedDestroyService)
        {
            _simpleDestroyService = simpleDestroyService;
            _animatedDestroyService = animatedDestroyService;
        }
        
        public async UniTask AsyncInitialize(IEnumerable<DestroyServiceData> param)
        {
            _destroyServices = param.ToDictionary(x => x.BoostTypeId, x => x.BlockDestroyService);

            await UniTask.CompletedTask;
        }

        public async void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            entityView.BoxCollider2D.enabled = false;
            if (_destroyServices.ContainsKey(gridItemData.BoostTypeId))
            {
                _destroyServices[gridItemData.BoostTypeId].Destroy(gridItemData, entityView);
                return;
            }

            await _animatedDestroyService.Animate(new()
            {
                new(gridItemData, entityView)
            });
            _simpleDestroyService.Destroy(gridItemData, entityView);
        }
    }
}