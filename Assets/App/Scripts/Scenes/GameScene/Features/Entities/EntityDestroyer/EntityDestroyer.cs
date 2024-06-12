using System.Collections.Generic;
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
        private Dictionary<string,IBlockDestroyService> _destroyServices;

        public EntityDestroyer(SimpleDestroyService simpleDestroyService, IAnimatedDestroyService animatedDestroyService)
        {
            _simpleDestroyService = simpleDestroyService;
            _animatedDestroyService = animatedDestroyService;
        }

        public UniTask AsyncInitialize(Dictionary<string, IBlockDestroyService> param)
        {
            _destroyServices = param;
            return UniTask.CompletedTask;
        }

        public async void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            entityView.BoxCollider2D.enabled = false;
            if (_destroyServices.ContainsKey(gridItemData.BoostTypeId.ToString()))
            {
                _destroyServices[gridItemData.BoostTypeId.ToString()].Destroy(gridItemData, entityView);
                return;
            }

            gridItemData.CurrentHealth = -1;
            await _animatedDestroyService.Animate(entityView);
            _simpleDestroyService.Destroy(gridItemData, entityView);
        }
    }
}