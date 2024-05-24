using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Settings;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices
{
    public class BallSpeedBoostsDestroyer : IBlockDestroyService
    {
        private readonly BoostsSettings _boostsSettings;
        private readonly IAnimatedDestroyService _animatedDestroyService;
        private readonly SimpleDestroyService _simpleDestroyService;
        private readonly BoostView.Factory _boostViewFactory;
        private readonly IBoostMoveService _boostMoveService;
        private readonly IBoostPositionChecker _boostPositionChecker;

        public BallSpeedBoostsDestroyer(
            BoostsSettings boostsSettings,
            IAnimatedDestroyService animatedDestroyService,
            SimpleDestroyService simpleDestroyService,
            BoostView.Factory boostViewFactory,
            IBoostMoveService boostMoveService,
            IBoostPositionChecker boostPositionChecker)
        {
            _boostsSettings = boostsSettings;
            _animatedDestroyService = animatedDestroyService;
            _simpleDestroyService = simpleDestroyService;
            _boostViewFactory = boostViewFactory;
            _boostMoveService = boostMoveService;
            _boostPositionChecker = boostPositionChecker;
        }
        
        public async void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            BoostView boostView = _boostViewFactory.Create(entityView.BoostTypeId);
            boostView.Transform.position = entityView.Position;
            
            _boostMoveService.AddView(boostView);
            _boostPositionChecker.Add(boostView);

            await _animatedDestroyService.Animate(new List<EntityData>()
            {
                new()
                {
                    GridItemData = null,
                    EntityView = entityView
                }
            });
            
            _simpleDestroyService.Destroy(gridItemData, entityView);
        }
    }
}