using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General
{
    public sealed class BoostPositionCheckerService : IBoostPositionChecker
    {
        private readonly BoostView.Pool _boostViewPool;
        private readonly IBoostMoveService _boostMoveService;

        private float _minYPosition;
        private List<BoostView> _views;

        public BoostPositionCheckerService(
            BoostView.Pool boostViewPool,
            IScreenInfoProvider screenInfoProvider,
            IBoostMoveService boostMoveService)
        {
            _boostViewPool = boostViewPool;
            _boostMoveService = boostMoveService;
            _views = new();
            
            _minYPosition = -screenInfoProvider.HeightInWorld / 2f - 1f;
        }
        
        public void Tick()
        {
            for(int i = 0; i < _views.Count; i++)
            {
                if (_views[i].Transform.position.y <= _minYPosition)
                {
                    _boostMoveService.RemoveView(_views[i]);

                    if (!_boostViewPool.InactiveItems.Contains(_views[i]))
                    {
                        _boostViewPool.Despawn(_views[i]);
                    }
                    
                    _views.Remove(_views[i]);
                    i--;
                }
            }
        }

        public void Add(BoostView view)
        {
            _views.Add(view);
        }
        
        public void Remove(BoostView view)
        {
            _views.Remove(view);
        }

        public void Restart()
        {
            _views.Clear();
        }
    }
}