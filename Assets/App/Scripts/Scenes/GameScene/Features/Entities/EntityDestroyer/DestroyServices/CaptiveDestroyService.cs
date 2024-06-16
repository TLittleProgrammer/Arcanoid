using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.Helpers;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices
{
    public class CaptiveDestroyService : IBlockDestroyService
    {
        private readonly BallView.Pool _ballViewPool;
        private readonly IBallsService _ballsService;
        private readonly IAnimatedDestroyService _animatedDestroyService;
        private readonly SimpleDestroyService _simpleDestroyService;

        public CaptiveDestroyService(
            BallView.Pool ballViewPool,
            IBallsService ballsService,
            IAnimatedDestroyService animatedDestroyService,
            SimpleDestroyService simpleDestroyService)
        {
            _ballViewPool = ballViewPool;
            _ballsService = ballsService;
            _animatedDestroyService = animatedDestroyService;
            _simpleDestroyService = simpleDestroyService;
        }

        public async void Destroy(GridItemData gridItemData, IEntityView entityView)
        {
            AddBall(entityView);

            gridItemData.CurrentHealth = -1;
            await _animatedDestroyService.Animate(entityView);
            _simpleDestroyService.Destroy(gridItemData, entityView);
        }

        private void AddBall(IEntityView entityView)
        {
            BallView ballView = _ballViewPool.Spawn();
            
            _ballsService.AddBall(ballView, true);
            
            ballView.Position = entityView.Position;
        }
    }
}