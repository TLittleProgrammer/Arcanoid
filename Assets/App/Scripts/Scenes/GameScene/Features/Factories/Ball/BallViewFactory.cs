using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories
{
    public class BallViewFactory : IFactory<BallView>
    {
        private readonly BallView.Pool _ballViewPool;

        public BallViewFactory(BallView.Pool ballViewPool)
        {
            _ballViewPool = ballViewPool;
        }
        
        public BallView Create()
        {
            BallView ballView = _ballViewPool.Spawn();
            
            return ballView;
        }
    }
}