using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Constants;
using App.Scripts.Scenes.GameScene.Features.Time;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts
{
    public class BoostMoveService : IBoostMoveService
    {
        private readonly ITimeProvider _timeProvider;

        private List<BoostView> _views;

        public BoostMoveService(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _views = new();
        }

        public void Tick()
        {
            if (!IsActive)
                return;
            
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

        public bool IsActive { get; set; }
    }
}