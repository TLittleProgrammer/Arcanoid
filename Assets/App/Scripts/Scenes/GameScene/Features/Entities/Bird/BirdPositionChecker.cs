using System;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird
{
    public class BirdPositionChecker : IBirdPositionChecker
    {
        public event Action<BirdView> BirdFlewAway;

        private readonly BirdView _bird;
        private readonly float _minXPosition;
        private readonly float _maxXPosition;
        private readonly float _deadLine = 1.25f;

        public BirdPositionChecker(BirdView bird, IScreenInfoProvider screenInfoProvider)
        {
            _bird = bird;
            
            _minXPosition = -screenInfoProvider.WidthInWorld / 2f - _deadLine;
            _maxXPosition = screenInfoProvider.WidthInWorld  / 2f + _deadLine;
        }

        public BirdView BirdView => _bird;

        public void Tick()
        {
            if (_bird.Transform.position.x >= _maxXPosition || _bird.Transform.position.x <= _minXPosition)
            {
                BirdFlewAway?.Invoke(_bird);
            }
        }
    }
}