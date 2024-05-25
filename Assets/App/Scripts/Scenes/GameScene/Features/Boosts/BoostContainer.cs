using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Time;

namespace App.Scripts.Scenes.GameScene.Features.Boosts
{
    public sealed class BoostContainer : IBoostContainer
    {
        private readonly BoostsSettings _boostsSettings;
        private readonly ITimeProvider _timeProvider;
        private readonly List<BoostData> _boosts;

        public event Action<BoostTypeId> BoostEnded;

        public BoostContainer(BoostsSettings boostsSettings, ITimeProvider timeProvider)
        {
            _boostsSettings = boostsSettings;
            _timeProvider = timeProvider;
            _boosts = new();
        }

        public void Tick()
        {
            for (int i = 0; i < _boosts.Count; i++)
            {
                _boosts[i].Duration -= _timeProvider.DeltaTime;

                if (_boosts[i].Duration <= 0f)
                {
                    BoostEnded?.Invoke(_boosts[i].BoostTypeId);
                    _boosts.RemoveAt(i);
                    i--;
                    
                    continue;
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
            BallSpeed(boostTypeId, BoostTypeId.BallAcceleration, BoostTypeId.BallSlowdown, _boostsSettings.BallSpeedDuration);
            BallSpeed(boostTypeId, BoostTypeId.PlayerShapeAddSize, BoostTypeId.PlayerShapeMinusSize, _boostsSettings.ShapeSizeDuration);
            BallSpeed(boostTypeId, BoostTypeId.PlayerShapeAddSpeed, BoostTypeId.PlayerShapeMinusSpeed, _boostsSettings.ShapeSpeedDuration);
        }
        
        private void BallSpeed(BoostTypeId boostTypeId, BoostTypeId firstType, BoostTypeId secondType, float duration)
        {
            if (boostTypeId == firstType)
            {
                if (_boosts.Count(x => x.BoostTypeId == secondType) != 0)
                {
                    BoostData boostData = _boosts.First(x => x.BoostTypeId == secondType);
                    _boosts.Remove(boostData);
                }

                _boosts.Add(new(firstType, duration));
            }

            if (boostTypeId == secondType)
            {
                if (_boosts.Count(x => x.BoostTypeId == firstType) != 0)
                {
                    BoostData boostData = _boosts.First(x => x.BoostTypeId == firstType);
                    _boosts.Remove(boostData);
                }

                _boosts.Add(new(secondType, duration));
            }
        }

        private void UpdateBoostDuration(BoostTypeId boostTypeId)
        {
            float targetDuration = boostTypeId switch
            {
                BoostTypeId.BallAcceleration => _boostsSettings.BallSpeedDuration,
                BoostTypeId.BallSlowdown => _boostsSettings.BallSpeedDuration,
                BoostTypeId.PlayerShapeAddSize => _boostsSettings.ShapeSizeDuration,
                BoostTypeId.PlayerShapeMinusSize => _boostsSettings.ShapeSizeDuration,
                BoostTypeId.PlayerShapeAddSpeed => _boostsSettings.ShapeSpeedDuration,
                BoostTypeId.PlayerShapeMinusSpeed => _boostsSettings.ShapeSpeedDuration,

                _ => 0f
            };

            _boosts.First(x => x.BoostTypeId == boostTypeId).Duration = targetDuration;
        }
    }
}