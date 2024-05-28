using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Factories;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices
{
    public class CaptiveDestroyService : IBlockDestroyService
    {
        private readonly BallView.Factory _ballViewFactory;
        private readonly IBallsService _ballsService;
        private readonly IAnimatedDestroyService _animatedDestroyService;
        private readonly SimpleDestroyService _simpleDestroyService;

        public CaptiveDestroyService(
            BallView.Factory ballViewFactory,
            IBallsService ballsService,
            IAnimatedDestroyService animatedDestroyService,
            SimpleDestroyService simpleDestroyService)
        {
            _ballViewFactory = ballViewFactory;
            _ballsService = ballsService;
            _animatedDestroyService = animatedDestroyService;
            _simpleDestroyService = simpleDestroyService;
        }
        
        public async void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            BallView ballView = _ballViewFactory.Create();

            ballView.Position = entityView.Position;
            _ballsService.AddBall(ballView, true);
            
            await _animatedDestroyService.Animate(new List<EntityData> { new(gridItemData, entityView) });
            _simpleDestroyService.Destroy(gridItemData, entityView);
        }
    }
}