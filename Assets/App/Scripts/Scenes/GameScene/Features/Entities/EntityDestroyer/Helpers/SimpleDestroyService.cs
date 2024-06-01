using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Entities.TopSprites;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers
{
    public class SimpleDestroyService
    {
        private readonly ILevelProgressService _levelProgressService;
        private readonly EntityView.Pool _entityViewPool;
        private readonly OnTopSprites.Pool _topSpritesPool;
        private readonly BoostView.Pool _boostViewPool;

        public SimpleDestroyService(
            ILevelProgressService levelProgressService,
            EntityView.Pool entityViewPool,
            OnTopSprites.Pool topSpritesPool,
            BoostView.Pool boostViewPool)
        {
            _levelProgressService = levelProgressService;
            _entityViewPool = entityViewPool;
            _topSpritesPool = topSpritesPool;
            _boostViewPool = boostViewPool;
        }
        
        public void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            EntityView entity = entityView as EntityView;
            
            if (gridItemData.CurrentHealth <= 0 && gridItemData.CanGetDamage && !_entityViewPool.InactiveItems.Contains(entity))
            {
                entityView.BoxCollider2D.enabled = false;
                _levelProgressService.TakeOneStep();

                _entityViewPool.Despawn(entity);
                foreach (OnTopSprites sprite in gridItemData.Sprites)
                {
                    _topSpritesPool.Despawn(sprite);
                }
            }
        }

        public void Destroy(BoostView view)
        {
            _boostViewPool.Despawn(view);
        }
    }
}