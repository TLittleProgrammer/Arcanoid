using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer;
using App.Scripts.Scenes.GameScene.Features.Levels.Load;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Activators
{
    public class FireballBoostActivator : IConcreteBoostActivator, ITickable
    {
        private readonly IBoostContainer _boostContainer;
        private readonly ILevelLoader _levelLoader;
        private readonly IItemsDestroyable _itemsDestroyable;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IBallsService _ballsService;
        private bool _isActive = false;
        
        public FireballBoostActivator(
            IBoostContainer boostContainer,
            ILevelLoader levelLoader,
            IItemsDestroyable itemsDestroyable,
            ILevelViewUpdater levelViewUpdater,
            IBallsService ballsService)
        {
            _boostContainer = boostContainer;
            _levelLoader = levelLoader;
            _itemsDestroyable = itemsDestroyable;
            _levelViewUpdater = levelViewUpdater;
            _ballsService = ballsService;

            _boostContainer.BoostEnded += OnBoostEnded;
        }

        public void Activate(BoostTypeId boostTypeId)
        {
            _ballsService.SetRedBall(true);
            _isActive = true;

            foreach (IEntityView view in _levelLoader.Entities)
            {
                if (view.GameObject.activeSelf)
                {
                    view.BoxCollider2D.isTrigger = true;
                }
            }
        }

        private void OnBoostEnded(BoostTypeId boostType)
        {
            if (boostType is BoostTypeId.Fireball)
            {
                foreach (IEntityView view in _levelLoader.Entities)
                {
                    view.BoxCollider2D.isTrigger = false;
                }

                _isActive = false;
                _ballsService.SetRedBall(false);
            }
        }

        public void Tick()
        {
            if (_isActive is false)
            {
                return;
            }

            CheckCollisions();
        }

        private void CheckCollisions()
        {
            foreach (IEntityView view in _levelLoader.Entities)
            {
                if (view.GameObject.activeSelf && view.BoxCollider2D.isTrigger)
                {
                    GoThroughBallViews(view);
                }
            }
        }

        private void GoThroughBallViews(IEntityView view)
        {
            List<BallView> ballViews = _ballsService.Balls.Keys.ToList();
            Bounds viewBounds = view.BoxCollider2D.bounds;

            foreach (BallView ballView in ballViews)
            {
                Bounds ballBounds = ballView.Collider2D.bounds;

                if (viewBounds.Intersects(ballBounds))
                {
                    DestroyView(view);
                }
            }
        }

        private void DestroyView(IEntityView view)
        {
            view.BoxCollider2D.isTrigger = false;
            view.BoxCollider2D.enabled = false;

            GridItemData gridItemData = _levelViewUpdater.LevelGridItemData[view.GridPositionX, view.GridPositionY];
            gridItemData.CurrentHealth = -1;
            _itemsDestroyable.Destroy(gridItemData, view);
        }
    }
}