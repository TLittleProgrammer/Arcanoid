using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.UI;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.Data;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Time;
using Object = UnityEngine.Object;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General
{
    public sealed class BoostContainer : IBoostContainer, ILevelProgressSavable, IInitializeByLevelProgress
    {
        private readonly BoostsSettings _boostsSettings;
        private readonly ITimeProvider _timeProvider;
        private readonly BoostItemView.Factory _boostItemFactory;
        private readonly BoostsViewContainer _boostsViewContainer;
        
        private List<BoostData> _boosts;
        private Dictionary<BoostTypeId, BoostItemView> _viewsDictionary = new();

        public bool IsActive { get; set; }
        public event Action<BoostTypeId> BoostEnded;

        public BoostContainer(
            BoostsSettings boostsSettings,
            ITimeProvider timeProvider,
            BoostItemView.Factory boostItemFactory,
            BoostsViewContainer boostsViewContainer
            )
        {
            _boostsSettings = boostsSettings;
            _timeProvider = timeProvider;
            _boostItemFactory = boostItemFactory;
            _boostsViewContainer = boostsViewContainer;
            _boosts = new();
        }

        public void Tick()
        {
            if (!IsActive)
            {
                return;
            }

            UpdateAllTimers();
        }

        private void UpdateAllTimers()
        {
            for (int i = 0; i < _boosts.Count; i++)
            {
                _boosts[i].Duration -= _timeProvider.DeltaTime;

                BoostTypeId currentBoostType = _boosts[i].BoostTypeId;

                UpdateView(currentBoostType, i);
                TryDestroyBoost(ref i, currentBoostType);
            }
        }

        private void UpdateView(BoostTypeId currentBoostType, int i)
        {
            _viewsDictionary[currentBoostType].ScollImage.fillAmount = _boosts[i].Duration / GetDurationByType(currentBoostType);
        }

        private void TryDestroyBoost(ref int i, BoostTypeId currentBoostType)
        {
            if (_boosts[i].Duration <= 0f)
            {
                RemoveItem(currentBoostType);
                BoostEnded?.Invoke(_boosts[i].BoostTypeId);
                _boosts.RemoveAt(i);
                i--;
            }
        }

        public void AddBoost(BoostTypeId boostTypeId)
        {
            if (TryUpdateTimerForActivatedBoosts(boostTypeId))
            {
                return;
            }

            CheckReplaceableBoosts(boostTypeId);
        }

        public bool BoostIsActive(BoostTypeId boostTypeId)
        {
            return _viewsDictionary.ContainsKey(boostTypeId);
        }

        private bool TryUpdateTimerForActivatedBoosts(BoostTypeId boostTypeId)
        {
            BoostData boostData = _boosts.FirstOrDefault(x => x.BoostTypeId == boostTypeId); 
            
            if (boostData is not null)
            {
                UpdateBoostDuration(boostData);
                return true;
            }

            return false;
        }

        private void CheckReplaceableBoosts(BoostTypeId boostTypeId)
        {
            if (TryReplaceBoosts(boostTypeId, BoostTypeId.BallAcceleration, BoostTypeId.BallSlowdown, _boostsSettings.BallSpeedDuration))
            {
                return;
            }

            if (TryReplaceBoosts(boostTypeId, BoostTypeId.PlayerShapeAddSize, BoostTypeId.PlayerShapeMinusSize, _boostsSettings.ShapeSizeDuration))
            {
                return;
            }

            if (TryReplaceBoosts(boostTypeId, BoostTypeId.PlayerShapeAddSpeed, BoostTypeId.PlayerShapeMinusSpeed, _boostsSettings.ShapeSpeedDuration))
            {
                return;
            }

            CreateBoost(boostTypeId);
        }

        private void CreateBoost(BoostTypeId boostTypeId)
        {
            BoostData boostData = new();
            boostData.BoostTypeId = boostTypeId;
            boostData.Duration = GetDurationByType(boostTypeId);

            _boosts.Add(boostData);
            CreateUiBoost(boostTypeId);
        }

        private bool TryReplaceBoosts(BoostTypeId boostTypeId, BoostTypeId firstType, BoostTypeId secondType, float duration)
        {
            bool firstRemovingResult = TryRemoveAndReplaceBoost(boostTypeId, firstType, secondType, duration);
            bool secondRemovingResult = TryRemoveAndReplaceBoost(boostTypeId, secondType, firstType, duration);

            return firstRemovingResult || secondRemovingResult;
        }

        private bool TryRemoveAndReplaceBoost(BoostTypeId inputBoost, BoostTypeId firstReplaceableBoost, BoostTypeId secondReplaceable, float duration)
        {
            if (inputBoost == firstReplaceableBoost)
            {
                BoostData boostData = _boosts.FirstOrDefault(x => x.BoostTypeId == secondReplaceable);
                if (boostData is not null)
                {
                    _boosts.Remove(boostData);
                    RemoveItem(secondReplaceable);
                }

                _boosts.Add(new(firstReplaceableBoost, duration));
                CreateUiBoost(firstReplaceableBoost);
                return true;
            }

            return false;
        }

        private void RemoveItem(BoostTypeId boostType)
        {
            Object.Destroy(_viewsDictionary[boostType].gameObject);
            _viewsDictionary.Remove(boostType);
        }

        private void CreateUiBoost(BoostTypeId boostType)
        {
            BoostItemView boostItemView = _boostItemFactory.Create(boostType);
            boostItemView.transform.SetParent(_boostsViewContainer.BoostsParent, false);
            
            _viewsDictionary.Add(boostType, boostItemView);
        }

        private void UpdateBoostDuration(BoostData boostData)
        {
            float targetDuration = GetDurationByType(boostData.BoostTypeId);

            boostData.Duration = targetDuration;
        }

        private float GetDurationByType(BoostTypeId boostTypeId)
        {
            return boostTypeId switch
            {
                BoostTypeId.BallAcceleration => _boostsSettings.BallSpeedDuration,
                BoostTypeId.BallSlowdown => _boostsSettings.BallSpeedDuration,
                BoostTypeId.PlayerShapeAddSize => _boostsSettings.ShapeSizeDuration,
                BoostTypeId.PlayerShapeMinusSize => _boostsSettings.ShapeSizeDuration,
                BoostTypeId.PlayerShapeAddSpeed => _boostsSettings.ShapeSpeedDuration,
                BoostTypeId.PlayerShapeMinusSpeed => _boostsSettings.ShapeSpeedDuration,
                BoostTypeId.Fireball => _boostsSettings.FireballDuration,
                BoostTypeId.StickyPlatform => _boostsSettings.StickyDuration,
                BoostTypeId.MiniGun => _boostsSettings.MiniGunDuration,
                BoostTypeId.Autopilot => _boostsSettings.AutopilotDuration,

                _ => 0f
            };
        }

        public void Restart()
        {
            IsActive = false;
            foreach (BoostData boostData in _boosts)
            {
                RemoveItem(boostData.BoostTypeId);
                BoostEnded?.Invoke(boostData.BoostTypeId);
            }
            
            _boosts.Clear();
        }

        public void SaveProgress(LevelDataProgress levelDataProgress)
        {
            levelDataProgress.ActiveBoostDatas = new();

            foreach (BoostData boostData in _boosts)
            {
                SaveActiveBoostData save = new(boostData);
                
                levelDataProgress.ActiveBoostDatas.Add(save);
            }
        }

        public void LoadProgress(LevelDataProgress levelDataProgress)
        {
            _boosts = new();

            foreach (SaveActiveBoostData save in levelDataProgress.ActiveBoostDatas)
            {
                BoostData boostData = new(save.BoostTypeId, save.Duration);
                CreateUiBoost(boostData.BoostTypeId);
                
                _boosts.Add(boostData);
            }
        }
    }
}