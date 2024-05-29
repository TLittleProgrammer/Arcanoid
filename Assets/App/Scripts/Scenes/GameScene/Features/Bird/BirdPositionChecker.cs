using System;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;

namespace App.Scripts.Scenes.GameScene.Features.Bird
{
    public class BirdPositionChecker : IBirdPositionChecker
    {
        public event Action<BirdView, IBirdPositionChecker> BirdFlewAway;

        private readonly BirdView _bird;
        private readonly float _minXPosition;
        private readonly float _maxXPosition;

        public BirdPositionChecker(BirdView bird, IScreenInfoProvider screenInfoProvider)
        {
            _bird = bird;

            _minXPosition = -screenInfoProvider.WidthInWorld / 2f - 1.25f;
            _maxXPosition = screenInfoProvider.WidthInWorld / 2f + 1.25f;
        }

        public BirdView BirdView => _bird;

        public void Tick()
        {
            if (_bird.Transform.position.x >= _maxXPosition || _bird.Transform.position.x <= _minXPosition)
            {
                BirdFlewAway?.Invoke(_bird, this);
            }
        }
    }
}