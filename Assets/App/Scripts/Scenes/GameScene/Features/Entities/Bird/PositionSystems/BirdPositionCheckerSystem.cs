using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird.PositionSystems
{
    public sealed class BirdPositionCheckerSystem : IBirdPositionCheckerSystem
    {
        private readonly IScreenInfoProvider _screenInfoProvider;
        
        private List<IBirdPositionChecker> _positionCheckers = new();

        public BirdPositionCheckerSystem(IScreenInfoProvider screenInfoProvider)
        {
            _screenInfoProvider = screenInfoProvider;
        }
        
        public bool IsActive { get; set; }

        public void Tick()
        {
            if (IsActive == false)
                return;

            CheckPositions();
        }

        private void CheckPositions()
        {
            for (int i = 0; i < _positionCheckers.Count; i++)
            {
                _positionCheckers[i].Tick();
            }
        }

        public IBirdPositionChecker AddBird(BirdView birdView)
        {
            IBirdPositionChecker birdPositionChecker = new BirdPositionChecker(birdView, _screenInfoProvider);
            _positionCheckers.Add(birdPositionChecker);

            return birdPositionChecker;
        }

        public void RemoveBird(BirdView birdView)
        {
            IBirdPositionChecker needRemove = _positionCheckers.FirstOrDefault(x => x.BirdView.Equals(birdView));
            _positionCheckers.Remove(needRemove);
        }
    }
}