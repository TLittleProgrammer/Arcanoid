﻿using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Time;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird
{
    public class BirdRespawnService : ILateTickable, IActivable
    {
        private readonly BirdSettings _birdSettings;
        private readonly ITimeProvider _timeProvider;
        private List<(BirdView, float)> _timeDictionary;

        private List<BirdView> _needRespawn;

        public event Action<BirdView> BirdRespawned;

        public BirdRespawnService(BirdSettings birdSettings, ITimeProvider timeProvider)
        {
            _birdSettings = birdSettings;
            _timeProvider = timeProvider;

            _timeDictionary = new();
            _needRespawn = new();
        }

        public bool IsActive { get; set; }

        public void LateTick()
        {
            if (!IsActive)
                return;

            UpdateRespawnTimers();
            RespawnAll();
        }

        private void RespawnAll()
        {
            foreach (BirdView birdView in _needRespawn)
            {
                BirdRespawned?.Invoke(birdView);
                birdView.CircleCollider2D.enabled = true;
            }
            _needRespawn.Clear();
        }

        private void UpdateRespawnTimers()
        {
            for (int i = 0; i < _timeDictionary.Count; i++)
            {
                (BirdView, float) tuple = _timeDictionary[i];
                tuple.Item2 -= _timeProvider.DeltaTime;

                _timeDictionary[i] = tuple;

                if (tuple.Item2 <= 0f)
                {
                    _needRespawn.Add(tuple.Item1);
                    _timeDictionary.Remove(tuple);
                    i--;
                }
            }
        }

        public void AddBirdToRespawn(BirdView birdView)
        {
            birdView.CircleCollider2D.enabled = false;
            _timeDictionary.Add((birdView, _birdSettings.RespawnTime));
        }

        public void StopAll()
        {
            foreach ((BirdView view, float time) in _timeDictionary)
            {
                view.CircleCollider2D.enabled = true;
            }

            _timeDictionary.Clear();
        }
    }
}