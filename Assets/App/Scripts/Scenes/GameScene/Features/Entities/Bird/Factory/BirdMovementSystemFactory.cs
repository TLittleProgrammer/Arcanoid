using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird.Factory
{
    public sealed class BirdMovementSystemFactory : IBirdMovementSystemFactory
    {
        private readonly DiContainer _sceneContext;

        public BirdMovementSystemFactory(DiContainer sceneContext)
        {
            _sceneContext = sceneContext;
        }
        
        public IBirdMovement Create(BirdView birdView)
        {
            return _sceneContext.Instantiate<BirdMovementSystem>(new[] { birdView });
        }
    }
}