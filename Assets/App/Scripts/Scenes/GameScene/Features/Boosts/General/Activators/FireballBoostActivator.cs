using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public class FireballBoostActivator : IConcreteBoostActivator, ITickable
    {
        private ILevelLoader _levelLoader;
        private IEntityDestroyable _entityDestroyable;
        private ILevelViewUpdater _levelViewUpdater;
        private IBallsService _ballsService;
        private IEffectActivator _effectActivator;
        private bool _isActive = false;
        
        public FireballBoostActivator(
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

        public bool IsTimeableBoost => true;

        public void Activate()
        {
            _ballsService.SetRedBall(true);
            _isActive = true;

            UpdateTrigger(true);
        }

        public void Tick()
        {
            if (_isActive is false)
            {
                return;
            }

            CheckCollisions();
        }

        public void Deactivate()
        {
            UpdateTrigger(false);

            _isActive = false;
            _ballsService.SetRedBall(false);
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

        private void UpdateTrigger(bool value)
        {
            foreach (IEntityView view in _levelLoader.Entities)
            {
                view.BoxCollider2D.isTrigger = value;
            }
        }
    }
}