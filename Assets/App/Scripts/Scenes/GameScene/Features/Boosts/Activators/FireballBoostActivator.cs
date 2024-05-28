using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer;
using App.Scripts.Scenes.GameScene.Features.Levels.Load;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Helpers
{
    public class FireballBoostActivator : IConcreteBoostActivator, ITickable
    {
        private readonly BallView _ballView;
        private readonly IBoostContainer _boostContainer;
        private readonly ILevelLoader _levelLoader;
        private readonly IItemsDestroyable _itemsDestroyable;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private bool _isActive = false;
        
        public FireballBoostActivator(
            BallView ballView,
            IBoostContainer boostContainer,
            ILevelLoader levelLoader,
            IItemsDestroyable itemsDestroyable,
            ILevelViewUpdater levelViewUpdater)
        {
            _ballView = ballView;
            _boostContainer = boostContainer;
            _levelLoader = levelLoader;
            _itemsDestroyable = itemsDestroyable;
            _levelViewUpdater = levelViewUpdater;

            _boostContainer.BoostEnded += OnBoostEnded;
        }

        public void Activate(BoostTypeId boostTypeId)
        {
            _ballView.RedBall.SetActive(true);
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
                _ballView.RedBall.SetActive(false);
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
                if (view.GameObject.activeSelf)
                {
                    if (view.BoxCollider2D.isTrigger)
                    {
                        Bounds viewBounds = view.BoxCollider2D.bounds;
                        Bounds ballBounds = _ballView.Collider2D.bounds;

                        if (viewBounds.Intersects(ballBounds))
                        {
                            view.BoxCollider2D.isTrigger = false;
                            view.BoxCollider2D.enabled = false;

                            GridItemData gridItemData = _levelViewUpdater.LevelGridItemData[view.GridPositionX, view.GridPositionY];
                            gridItemData.CurrentHealth = -1;
                            _itemsDestroyable.Destroy(gridItemData, view);
                        }
                    }
                }
            }
        }
    }
}