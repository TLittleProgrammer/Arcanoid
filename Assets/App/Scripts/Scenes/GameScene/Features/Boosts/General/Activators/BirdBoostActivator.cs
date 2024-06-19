using App.Scripts.Scenes.GameScene.Features.Entities.Bird;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Factories;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public class BirdBoostActivator : IConcreteBoostActivator
    {
        private readonly IBirdsService _birdsService;
        private readonly BirdView.Factory _birdViewFactory;

        public BirdBoostActivator(IBirdsService birdsService, BirdView.Factory birdViewFactory)
        {
            _birdsService = birdsService;
            _birdViewFactory = birdViewFactory;
        }
        
        public bool IsTimeableBoost => false;
        
        public void Activate()
        {
            BirdView birdView = _birdViewFactory.Create();
            _birdsService.AddBird(birdView);
        }

        public void Deactivate()
        {
        }
    }
}