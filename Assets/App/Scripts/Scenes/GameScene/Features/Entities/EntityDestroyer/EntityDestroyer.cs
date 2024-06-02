using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer
{
    public sealed class EntityDestroyer : IEntityDestroyable
    {
        private readonly SimpleDestroyService _simpleDestroyService;
        private readonly IAnimatedDestroyService _animatedDestroyService;

        private Dictionary<BoostTypeId,IBlockDestroyService> _destroyServices;

        public EntityDestroyer(SimpleDestroyService simpleDestroyService, IAnimatedDestroyService animatedDestroyService)
        {
            _simpleDestroyService = simpleDestroyService;
            _animatedDestroyService = animatedDestroyService;
        }
        
        public async UniTask AsyncInitialize(List<DestroyServiceData> param)
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

            await _animatedDestroyService.Animate(entityView);
            _simpleDestroyService.Destroy(gridItemData, entityView);
        }
    }
}