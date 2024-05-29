using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices
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

        public BoostTypeId[] ProccessingBoostTypes => new[]
        {
            BoostTypeId.CaptiveBall
        };

        public async void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            AddBall(entityView);

            await _animatedDestroyService.Animate(new List<EntityData> { new(gridItemData, entityView) });
            _simpleDestroyService.Destroy(gridItemData, entityView);
        }

        private void AddBall(IEntityView entityView)
        {
            BallView ballView = _ballViewFactory.Create();

            ballView.Position = entityView.Position;
            _ballsService.AddBall(ballView, true);
        }
    }
}