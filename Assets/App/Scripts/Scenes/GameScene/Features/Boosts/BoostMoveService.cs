using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Constants;
using App.Scripts.Scenes.GameScene.Features.Pools;
using App.Scripts.Scenes.GameScene.Features.Time;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts
{
    public class BoostMoveService : IBoostMoveService
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IPoolContainer _poolContainer;

        private List<BoostView> _views;

        public BoostMoveService(ITimeProvider timeProvider, IPoolContainer poolContainer)
        {
            _timeProvider = timeProvider;
            _poolContainer = poolContainer;
            _views = new();
        }

        public void Tick()
        {
            foreach (BoostView view in _views)
            {
                Vector3 delta = Vector2.down * _timeProvider.DeltaTime * BoostsConstants.BoostSpeed;
                view.Transform.position += delta;
            }
        }

        public void AddView(BoostView boostView)
        {
            _views.Add(boostView);
        }

        public void RemoveView(BoostView boostView)
        {
            _views.Remove(boostView);
        }
    }
}