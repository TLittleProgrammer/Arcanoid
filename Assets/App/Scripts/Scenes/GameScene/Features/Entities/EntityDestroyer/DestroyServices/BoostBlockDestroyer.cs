using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices
{
    public class BoostBlockDestroyer : IBlockDestroyService, IInitializeByLevelProgress
    {
        private readonly IAnimatedDestroyService _animatedDestroyService;
        private readonly SimpleDestroyService _simpleDestroyService;
        private readonly BoostView.Factory _boostViewFactory;
        private readonly IBoostMoveService _boostMoveService;
        private readonly IBoostPositionChecker _boostPositionChecker;

        public BoostBlockDestroyer(
            IAnimatedDestroyService animatedDestroyService,
            SimpleDestroyService simpleDestroyService,
            BoostView.Factory boostViewFactory,
            IBoostMoveService boostMoveService,
            IBoostPositionChecker boostPositionChecker)
        {
            _animatedDestroyService = animatedDestroyService;
            _simpleDestroyService = simpleDestroyService;
            _boostViewFactory = boostViewFactory;
            _boostMoveService = boostMoveService;
            _boostPositionChecker = boostPositionChecker;
        }

        public BoostTypeId[] ProccessingBoostTypes
        {
            get => new[]
            {
                BoostTypeId.BallAcceleration,
                BoostTypeId.BallSlowdown,
                BoostTypeId.PlayerShapeAddSize,
                BoostTypeId.PlayerShapeMinusSize,
                BoostTypeId.PlayerShapeAddSpeed,
                BoostTypeId.PlayerShapeMinusSpeed,
                BoostTypeId.AddHealth,
                BoostTypeId.MinusHealth,
                BoostTypeId.Fireball,
                BoostTypeId.StickyPlatform,
                BoostTypeId.MiniGun,
                BoostTypeId.Autopilot,
            };
        }

        public void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            AddBoostOnMap(entityView.BoostTypeId, entityView.Position);
            DestroyBoostBlock(gridItemData, entityView).Forget();
        }

        private async UniTask DestroyBoostBlock(GridItemData gridItemData, IEntityView entityView)
        {
            await _animatedDestroyService.Animate(new List<EntityData>()
            {
                new()
                {
                    GridItemData = null,
                    EntityView = entityView
                }
            });
            gridItemData.CurrentHealth = 0;

            _simpleDestroyService.Destroy(gridItemData, entityView);
        }

        public void LoadProgress(LevelDataProgress levelDataProgress)
        {
            foreach (SaveBoostViewData data in levelDataProgress.ViewBoostDatas)
            {
                AddBoostOnMap(data.BoostTypeId, new Vector2(data.PositionX, data.PositionY));
            }
        }

        private void AddBoostOnMap(BoostTypeId boostTypeId, Vector2 position)
        {
            BoostView boostView = _boostViewFactory.Create(boostTypeId);
            boostView.Transform.position = position;

            _boostMoveService.AddView(boostView);
            _boostPositionChecker.Add(boostView);
        }
    }
}