using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Constants;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data;
using App.Scripts.Scenes.GameScene.Features.Time;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General
{
    public class BoostMoveService : IBoostMoveService, ILevelProgressSavable
    {
        private readonly ITimeProvider _timeProvider;
        private readonly BoostView.Pool _boostViewPool;

        private List<BoostView> _views;

        public BoostMoveService(ITimeProvider timeProvider, BoostView.Pool boostViewPool)
        {
            _timeProvider = timeProvider;
            _boostViewPool = boostViewPool;
            _views = new();
        }

        public bool IsActive { get; set; }

        public List<BoostView> Views => _views;

        public void Tick()
        {
            if (!IsActive)
                return;

            UpdateViewPositions();
        }

        private void UpdateViewPositions()
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

        public void SaveProgress(LevelDataProgress levelDataProgress)
        {
            levelDataProgress.ViewBoostDatas = new();

            foreach (BoostView view in _views)
            {
                SaveBoostViewData saveBoostViewData = new(view);
                
                levelDataProgress.ViewBoostDatas.Add(saveBoostViewData);
            }
        }

        public void Restart()
        {
            foreach (BoostView view in _views)
            {
                if (view.gameObject.activeSelf)
                {
                    _boostViewPool.Despawn(view);
                }
            }
            
            _views.Clear();
        }
    }
}