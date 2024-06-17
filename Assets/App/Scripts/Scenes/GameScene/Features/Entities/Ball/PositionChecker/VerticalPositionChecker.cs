using System;
using App.Scripts.Scenes.GameScene.Features.Components;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.PositionChecker
{
    public class VerticalPositionChecker : IPositionChecker
    {
        private readonly Func<IPositionable, float, bool> _condition;
        private readonly IPositionable _positionable;
        private readonly float _yPosition;

        public event Action<IPositionable, IPositionChecker> WentAbroad;

        public VerticalPositionChecker(Func<IPositionable, float, bool> condition, IPositionable positionable, float yPosition)
        {
            _condition = condition;
            _positionable = positionable;
            _yPosition = yPosition;
        }

        public IPositionable Positionable => _positionable;

        public void Tick()
        {
            if (_condition.Invoke(_positionable, _yPosition))
            {
                WentAbroad?.Invoke(_positionable, this);
            }
        }
    }
}