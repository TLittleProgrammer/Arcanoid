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
            if (boostTypeId is BoostTypeId.BallSlowdown)
            {
                if (_boosts.Count(x => x.BoostTypeId == BoostTypeId.BallAcceleration) != 0)
                {
                    BoostData boostData = _boosts.First(x => x.BoostTypeId == BoostTypeId.BallAcceleration);
                    _boosts.Remove(boostData);
                }

                _boosts.Add(new(BoostTypeId.BallSlowdown, _boostsSettings.BallSpeedDuration));
            }

            if (boostTypeId is BoostTypeId.BallAcceleration)
            {
                if (_boosts.Count(x => x.BoostTypeId == BoostTypeId.BallSlowdown) != 0)
                {
                    BoostData boostData = _boosts.First(x => x.BoostTypeId == BoostTypeId.BallSlowdown);
                    _boosts.Remove(boostData);
                }

                _boosts.Add(new(BoostTypeId.BallAcceleration, _boostsSettings.BallSpeedDuration));
            }
        }

        private void UpdateBoostDuration(BoostTypeId boostTypeId)
        {
            float targetDuration = boostTypeId switch
            {
                BoostTypeId.BallAcceleration => _boostsSettings.BallSpeedDuration,
                BoostTypeId.BallSlowdown => _boostsSettings.BallSpeedDuration,

                _ => 0f
            };

            _boosts.First(x => x.BoostTypeId == boostTypeId).Duration = targetDuration;
        }
    }
}