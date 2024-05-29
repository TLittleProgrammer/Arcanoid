using App.Scripts.Scenes.GameScene.Features.Entities.Bird;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories
{
    public class BirdViewFactory : IFactory<BirdView>
    {
        private readonly DiContainer _sceneContext;
        private readonly BirdView.Pool _birdViewPool;
        private readonly IBirdsService _birdsService;

        public BirdViewFactory(
            DiContainer sceneContext,
            BirdView.Pool birdViewPool,
            IBirdsService birdsService)
        {
            _sceneContext = sceneContext;
            _birdViewPool = birdViewPool;
            _birdsService = birdsService;
        }
        
        public BirdView Create()
        {
            BirdView birdView = _birdViewPool.Spawn();

            if (!_birdsService.Birds.ContainsKey(birdView))
            {
                IBirdMovement birdMovement = _sceneContext.Instantiate<BirdMovementService>(new[] { birdView });
                
                _birdsService.Birds.Add(birdView, birdMovement);
            }

            return birdView;
        }
    }
}