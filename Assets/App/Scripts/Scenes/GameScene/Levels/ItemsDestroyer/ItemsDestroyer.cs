using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Ball;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.LevelProgress;
using App.Scripts.Scenes.GameScene.Levels.Data;
using App.Scripts.Scenes.GameScene.Levels.ItemsDestroyer.DestroyServices;
using App.Scripts.Scenes.GameScene.Pools;
using App.Scripts.Scenes.GameScene.TopSprites;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Levels.ItemsDestroyer
{
    public sealed class ItemsDestroyer : IItemsDestroyable
    {
        private readonly ILevelProgressService _levelProgressService;
        private readonly IPoolContainer _poolContainer;
        private readonly IBallSpeedUpdater _ballSpeedUpdater;
        
        private Dictionary<BoostTypeId,IBlockDestroyService> _destroyServices;

        public ItemsDestroyer(
            ILevelProgressService levelProgressService,
            IPoolContainer poolContainer,
            IBallSpeedUpdater ballSpeedUpdater)
        {
            _levelProgressService = levelProgressService;
            _poolContainer = poolContainer;
            _ballSpeedUpdater = ballSpeedUpdater;
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

            SimpleDestroy(gridItemData, entityView);
        }

        private void SimpleDestroy(GridItemData gridItemData, IEntityView entityView)
        {
            _levelProgressService.TakeOneStep();

            _poolContainer.RemoveItem(PoolTypeId.EntityView, entityView as EntityView);
            foreach (OnTopSprites sprite in gridItemData.Sprites)
            {
                _poolContainer.RemoveItem(PoolTypeId.OnTopSprite, sprite);
            }

            _ballSpeedUpdater.UpdateSpeed();
        }
    }
}