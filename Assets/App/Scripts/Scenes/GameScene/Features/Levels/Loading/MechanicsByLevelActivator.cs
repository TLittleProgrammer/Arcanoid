using App.Scripts.Scenes.GameScene.Features.Entities.Bird;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Levels.General;

namespace App.Scripts.Scenes.GameScene.Features.Levels.Loading
{
    public sealed class MechanicsByLevelActivator : IMechanicsByLevelActivator
    {
        private readonly BirdView.Factory _birdViewFactory;
        private readonly IBirdsService _birdsService;

        public MechanicsByLevelActivator(BirdView.Factory birdViewFactory, IBirdsService birdsService)
        {
            _birdViewFactory = birdViewFactory;
            _birdsService = birdsService;
        }
        
        public void ActivateByLevelData(LevelData levelData)
        {
            TryActivateBirdMechanic(levelData);
        }

        private void TryActivateBirdMechanic(LevelData levelData)
        {
            if (levelData.NeedBird)
            {
                BirdView birdView = _birdViewFactory.Create();
                _birdsService.AddBird(birdView);
                _birdsService.EnableView(birdView);
            }
            else
            {
                _birdsService.StopAll();
            }
        }
    }
}