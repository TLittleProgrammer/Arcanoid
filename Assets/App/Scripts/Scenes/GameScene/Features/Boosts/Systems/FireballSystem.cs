using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Systems
{
    public class FireballSystem : IFireballSystem
    {
        private readonly ILevelLoader _levelLoader;
        private readonly IEntityDestroyable _entityDestroyable;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IBallsService _ballsService;
        private readonly IEffectActivator _effectActivator;

        public FireballSystem(
            ILevelLoader levelLoader,
            IEntityDestroyable entityDestroyable,
            ILevelViewUpdater levelViewUpdater,
            IBallsService ballsService,
            IEffectActivator effectActivator)
        {
            _levelLoader = levelLoader;
            _entityDestroyable = entityDestroyable;
            _levelViewUpdater = levelViewUpdater;
            _ballsService = ballsService;
            _effectActivator = effectActivator;
        }
        
        public bool IsActive { get; set; }

        public void Tick()
        {
            if (IsActive is false)
            {
                return;
            }

            CheckCollisions();
        }
        
        private void CheckCollisions()
        {
            foreach (IEntityView view in _levelLoader.Entities)
            {
                if (view.GameObject.activeSelf)
                {
                    GoThroughBallViews(view);
                }
            }
        }

        private void GoThroughBallViews(IEntityView view)
        {
            Bounds viewBounds = view.BoxCollider2D.bounds;

            foreach (BallView ballView in _ballsService.Balls.ToArray())
            {
                if (!ballView.gameObject.activeSelf)
                {
                    continue;
                }
                
                Bounds ballBounds = ballView.Collider2D.bounds;

                if (viewBounds.Intersects(ballBounds) && _levelViewUpdater.LevelGridItemData[view.GridPositionX, view.GridPositionY].CurrentHealth > 0)
                {
                    DestroyView(view, ballView);
                }
            }
        }
        
        private void DestroyView(IEntityView view, BallView ballView)
        {
            view.BoxCollider2D.isTrigger = false;
            view.BoxCollider2D.enabled = false;

            GridItemData gridItemData = _levelViewUpdater.LevelGridItemData[view.GridPositionX, view.GridPositionY];
            gridItemData.CurrentHealth = -1;
            
            _effectActivator.ActivateEffect(view, ballView.Collider2D);
            
            _entityDestroyable.Destroy(gridItemData, view);
        }
    }
}