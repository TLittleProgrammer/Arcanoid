using App.Scripts.Scenes.GameScene.Features.Entities.Bird;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories
{
    public class BirdViewFactory : IFactory<BirdView>
    {
        private readonly BirdView.Pool _birdViewPool;

        public BirdViewFactory(BirdView.Pool birdViewPool)
        {
            _birdViewPool = birdViewPool;
        }
        
        public BirdView Create()
        {
            BirdView birdView = _birdViewPool.Spawn();

            return birdView;
        }
    }
}