using System;
using App.Scripts.Scenes.GameScene.Features.Components;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.PositionChecker
{
    public class BallPositionChecker : IBallPositionChecker
    {
        private readonly IPositionable _ball;
        private readonly float _minYPosition;

        public event Action<IPositionable> BallFallen;

        public BallPositionChecker(IPositionable ball, float minYPosition)
        {
            _ball = ball;
            _minYPosition = minYPosition;
        }

        public IPositionable BallPositionable => _ball;

        public void Tick()
        {
            if (_ball.Position.y <= _minYPosition)
            {
                BallFallen?.Invoke(_ball);
            }
        }
    }
}