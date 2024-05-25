using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Pools;
using App.Scripts.Scenes.GameScene.Features.TopSprites;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers
{
    public class SimpleDestroyService
    {
        private readonly ILevelProgressService _levelProgressService;
        private readonly IPoolContainer _poolContainer;
        private readonly IBallSpeedUpdater _ballSpeedUpdater;

        public SimpleDestroyService(
            ILevelProgressService levelProgressService,
            IPoolContainer poolContainer,
            IBallSpeedUpdater ballSpeedUpdater)
        {
            _levelProgressService = levelProgressService;
            _poolContainer = poolContainer;
            _ballSpeedUpdater = ballSpeedUpdater;
        }
        
        public void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            if (gridItemData.CurrentHealth <= 0 && gridItemData.CanGetDamage)
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

        public void Destroy(BoostView view)
        {
            _poolContainer.RemoveItem(PoolTypeId.Boosts, view);
        }
    }
}