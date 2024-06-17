using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.PositionChecker;
using UnityEditor.Experimental.GraphView;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.PositionCheckers
{
    public sealed class PositionCheckerSystem : IPositionCheckerSystem
    {
        private readonly List<IPositionChecker> _positionCheckers = new();
        
        public void Tick()
        {
            for (int i = 0; i < _positionCheckers.Count; i++)
            {
                _positionCheckers[i].Tick();
            }
        }

        public void Add(IPositionChecker positionChecker)
        {
            _positionCheckers.Add(positionChecker);
        }

        public void Remove(IPositionChecker positionChecker)
        {
            _positionCheckers.Remove(positionChecker);
        }

        public void RemoveAllByPositionable(List<IPositionable> positionables)
        {
            foreach (IPositionable positionable in positionables)
            {
                var result = _positionCheckers.FirstOrDefault(x => x.Positionable.Equals(positionable));

                if (result is not null)
                {
                    _positionCheckers.Remove(result);
                }
            }
        }
    }
}