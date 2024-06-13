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
        private readonly Dictionary<string, BoostSettingsData> _boostSettingsDatas;

        private List<BoostData> _boosts;
        private Dictionary<string, BoostItemView> _viewsDictionary = new();

        public bool IsActive { get; set; }
        public event Action<string> BoostEnded;

        public BoostContainer(
            BoostsSettings boostsSettings,
            ITimeProvider timeProvider,
            BoostItemView.Factory boostItemFactory,
            BoostsViewContainer boostsViewContainer,
            Dictionary<string, BoostSettingsData> boostSettingsDatas
            )
        {
            _boostsSettings = boostsSettings;
            _timeProvider = timeProvider;
            _boostItemFactory = boostItemFactory;
            _boostsViewContainer = boostsViewContainer;
            _boostSettingsDatas = boostSettingsDatas;
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

                string currentBoostType = _boosts[i].BoostTypeId;

                UpdateView(currentBoostType, i);
                TryDestroyBoost(ref i, currentBoostType);
            }
        }

        private void UpdateView(string currentBoostType, int i)
        {
            _viewsDictionary[currentBoostType].ScollImage.fillAmount = _boosts[i].Duration / _boostSettingsDatas[currentBoostType].Duration;
        }

        private void TryDestroyBoost(ref int i, string currentBoostType)
        {
            if (_boosts[i].Duration <= 0f)
            {
                RemoveItem(currentBoostType);
                BoostEnded?.Invoke(_boosts[i].BoostTypeId);
                _boosts.RemoveAt(i);
                i--;
            }
        }

        public void AddBoost(string boostTypeId)
        {
            if (TryUpdateTimerForActivatedBoosts(boostTypeId))
            {
                return;
            }

            CheckReplaceableBoosts(boostTypeId);
        }

        public bool BoostIsActive(string boostTypeId)
        {
            return _viewsDictionary.ContainsKey(boostTypeId);
        }

        private bool TryUpdateTimerForActivatedBoosts(string boostTypeId)
        {
            BoostData boostData = _boosts.FirstOrDefault(x => x.BoostTypeId == boostTypeId); 
            
            if (boostData is not null)
            {
                UpdateBoostDuration(boostData);
                return true;
            }

            return false;
        }

        private void CheckReplaceableBoosts(string boostTypeId)
        {
            DeleteBoosts(boostTypeId);
            CreateBoost(boostTypeId);
        }

        private void CreateBoost(string boostTypeId)
        {
            BoostData boostData = new();
            boostData.BoostTypeId = boostTypeId;
            boostData.Duration = _boostSettingsDatas[boostTypeId].Duration;

            _boosts.Add(boostData);
            CreateUiBoost(boostTypeId);
        }

        private void DeleteBoosts(string boostTypeId)
        {
            foreach (BoostSettingsData settingsData in _boostSettingsDatas.Values)
            {
                if (_viewsDictionary.ContainsKey(settingsData.Key) && settingsData.KeysThatCanBlockThisBoost.Contains(boostTypeId))
                {
                    var boostData = _boosts.First(x => x.BoostTypeId.Equals(settingsData.Key));
                    _boosts.Remove(boostData);
                    RemoveItem(settingsData.Key);
                }
            }
        }

        private void RemoveItem(string boostType)
        {
            Object.Destroy(_viewsDictionary[boostType].gameObject);
            _viewsDictionary.Remove(boostType);
        }

        private void CreateUiBoost(string boostType)
        {
            BoostItemView boostItemView = _boostItemFactory.Create(boostType);
            boostItemView.transform.SetParent(_boostsViewContainer.BoostsParent, false);
            
            _viewsDictionary.Add(boostType, boostItemView);
        }

        private void UpdateBoostDuration(BoostData boostData)
        {
            float targetDuration = _boostSettingsDatas[boostData.BoostTypeId].Duration;

            boostData.Duration = targetDuration;
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