using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Boosts.UI;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Time;
using Object = UnityEngine.Object;

namespace App.Scripts.Scenes.GameScene.Features.Boosts
{
    public sealed class BoostContainer : IBoostContainer
    {
        private readonly BoostsSettings _boostsSettings;
        private readonly ITimeProvider _timeProvider;
        private readonly BoostItemView.Factory _boostItemFactory;
        private readonly BoostsViewContainer _boostsViewContainer;
        private readonly List<BoostData> _boosts;
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
            
            for (int i = 0; i < _boosts.Count; i++)
            {
                _boosts[i].Duration -= _timeProvider.DeltaTime;

                BoostTypeId currentBoostType = _boosts[i].BoostTypeId;

                _viewsDictionary[currentBoostType].ScollImage.fillAmount = _boosts[i].Duration / GetDurationByType(currentBoostType);

                if (_boosts[i].Duration <= 0f)
                {
                    RemoveItem(currentBoostType);
                    BoostEnded?.Invoke(_boosts[i].BoostTypeId);
                    _boosts.RemoveAt(i);
                    i--;
                }
            }
        }

        public void AddActive(BoostTypeId boostTypeId)
        {
            if (_boosts.Count(x => x.BoostTypeId == boostTypeId) != 0)
            {
                UpdateBoostDuration(boostTypeId);
                return;
            }

            CheckBallSpeedBoost(boostTypeId);
        }

        private void CheckBallSpeedBoost(BoostTypeId boostTypeId)
        {
            if (UpdateDurationForBoosts(boostTypeId, BoostTypeId.Fireball, _boostsSettings.FireballDuration)) return;
            if (UpdateDurationForBoosts(boostTypeId, BoostTypeId.StickyPlatform, _boostsSettings.StickyDuration)) return;
            if (UpdateDurationForBoosts(boostTypeId, BoostTypeId.MiniGun, _boostsSettings.MiniGunDuration)) return;
            if (UpdateDurationForBoosts(boostTypeId, BoostTypeId.Autopilot, _boostsSettings.AutopilotDuration)) return;
            
            BallSpeed(boostTypeId, BoostTypeId.BallAcceleration, BoostTypeId.BallSlowdown, _boostsSettings.BallSpeedDuration);
            BallSpeed(boostTypeId, BoostTypeId.PlayerShapeAddSize, BoostTypeId.PlayerShapeMinusSize, _boostsSettings.ShapeSizeDuration);
            BallSpeed(boostTypeId, BoostTypeId.PlayerShapeAddSpeed, BoostTypeId.PlayerShapeMinusSpeed, _boostsSettings.ShapeSpeedDuration);
        }

        private bool UpdateDurationForBoosts(BoostTypeId boostTypeId, BoostTypeId checkBoost, float duration)
        {
            if (boostTypeId == checkBoost)
            {
                if (_boosts.Count(x => x.BoostTypeId == checkBoost) != 0)
                {
                    UpdateBoostDuration(checkBoost);
                }
                else
                {
                    _boosts.Add(new(checkBoost, duration));
                    CreateUiBoost(checkBoost);
                }

                return true;
            }

            return false;
        }

        private void BallSpeed(BoostTypeId boostTypeId, BoostTypeId firstType, BoostTypeId secondType, float duration)
        {
            if (boostTypeId == firstType)
            {
                if (_boosts.Count(x => x.BoostTypeId == secondType) != 0)
                {
                    BoostData boostData = _boosts.First(x => x.BoostTypeId == secondType);
                    _boosts.Remove(boostData);
                    RemoveItem(secondType);
                }

                _boosts.Add(new(firstType, duration));
                CreateUiBoost(firstType);
            }

            if (boostTypeId == secondType)
            {
                if (_boosts.Count(x => x.BoostTypeId == firstType) != 0)
                {
                    BoostData boostData = _boosts.First(x => x.BoostTypeId == firstType);
                    _boosts.Remove(boostData);
                    RemoveItem(firstType);
                }

                _boosts.Add(new(secondType, duration));
                CreateUiBoost(secondType);
            }
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

        private void UpdateBoostDuration(BoostTypeId boostTypeId)
        {
            float targetDuration = GetDurationByType(boostTypeId);

            _boosts.First(x => x.BoostTypeId == boostTypeId).Duration = targetDuration;
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
    }
}