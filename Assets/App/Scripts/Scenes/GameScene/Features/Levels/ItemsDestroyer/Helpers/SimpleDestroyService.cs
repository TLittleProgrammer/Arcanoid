using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Entities.TopSprites;
using App.Scripts.Scenes.GameScene.Features.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Pools;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers
{
    public class SimpleDestroyService
    {
        private readonly ILevelProgressService _levelProgressService;
        private readonly IPoolContainer _poolContainer;

        public SimpleDestroyService(ILevelProgressService levelProgressService, IPoolContainer poolContainer)
        {
            _levelProgressService = levelProgressService;
            _poolContainer = poolContainer;
        }
        
        public void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            if (gridItemData.CurrentHealth <= 0 && gridItemData.CanGetDamage)
            {
                entityView.BoxCollider2D.enabled = false;
                _levelProgressService.TakeOneStep();

                _poolContainer.RemoveItem(PoolTypeId.EntityView, entityView as EntityView);
                foreach (OnTopSprites sprite in gridItemData.Sprites)
                {
                    _poolContainer.RemoveItem(PoolTypeId.OnTopSprite, sprite);
                }
            }
        }

        public void Destroy(BoostView view)
        {
            _poolContainer.RemoveItem(PoolTypeId.Boosts, view);
        }
    }
}