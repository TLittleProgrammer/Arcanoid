using System.Collections.Generic;
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

        public BallSpeedBoostsDestroyer(
            BoostsSettings boostsSettings,
            IAnimatedDestroyService animatedDestroyService,
            SimpleDestroyService simpleDestroyService)
        {
            _boostsSettings = boostsSettings;
            _animatedDestroyService = animatedDestroyService;
            _simpleDestroyService = simpleDestroyService;
        }
        
        public async void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
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