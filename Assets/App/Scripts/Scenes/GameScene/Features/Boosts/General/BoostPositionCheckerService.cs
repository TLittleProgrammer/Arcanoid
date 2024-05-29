using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Pools;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General
{
    public sealed class BoostPositionCheckerService : IBoostPositionChecker
    {
        private readonly IPoolContainer _poolContainer;
        private readonly IBoostMoveService _boostMoveService;

        private float _minYPosition;
        private List<BoostView> _views;

        public BoostPositionCheckerService(
            IPoolContainer poolContainer,
            IScreenInfoProvider screenInfoProvider,
            IBoostMoveService boostMoveService)
        {
            _poolContainer = poolContainer;
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
                    _poolContainer.RemoveItem(PoolTypeId.Boosts, _views[i]);
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
    }
}