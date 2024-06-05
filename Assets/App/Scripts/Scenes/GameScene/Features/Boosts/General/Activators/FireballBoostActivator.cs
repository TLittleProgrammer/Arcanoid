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
        private readonly IBoostContainer _boostContainer;
        private readonly ILevelLoader _levelLoader;
        private readonly IEntityDestroyable _entityDestroyable;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IBallsService _ballsService;
        private bool _isActive = false;
        
        public FireballBoostActivator(
            IBoostContainer boostContainer,
            ILevelLoader levelLoader,
            IEntityDestroyable entityDestroyable,
            ILevelViewUpdater levelViewUpdater,
            IBallsService ballsService)
        {
            _boostContainer = boostContainer;
            _levelLoader = levelLoader;
            _entityDestroyable = entityDestroyable;
            _levelViewUpdater = levelViewUpdater;
            _ballsService = ballsService;

            _boostContainer.BoostEnded += OnBoostEnded;
        }

        public void Activate(BoostTypeId boostTypeId)
        {
            _ballsService.SetRedBall(true);
            _isActive = true;

            UpdateTrigger(true);
        }

        private void OnBoostEnded(BoostTypeId boostType)
        {
            if (boostType is BoostTypeId.Fireball)
            {
                UpdateTrigger(false);

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
                if (view.GameObject.activeSelf)
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
                if (!ballView.gameObject.activeSelf)
                {
                    continue;
                }
                
                Bounds ballBounds = ballView.Collider2D.bounds;

                if (viewBounds.Intersects(ballBounds) && _levelViewUpdater.LevelGridItemData[view.GridPositionX, view.GridPositionY].CurrentHealth > 0)
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