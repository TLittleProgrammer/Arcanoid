using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Constants;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Time;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General
{
    public class BoostMoveService : IBoostMoveService, ILevelProgressSavable
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
        
        public void SaveProgress(LevelDataProgress levelDataProgress)
        {
            levelDataProgress.ViewBoostDatas = new();

            foreach (BoostView view in _views)
            {
                SaveBoostViewData saveBoostViewData = new(view);
                
                levelDataProgress.ViewBoostDatas.Add(saveBoostViewData);
            }
        }
    }
}