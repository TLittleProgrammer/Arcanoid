using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Time;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Bird
{
    public class BirdRespawnService : ILateTickable, IActivable
    {
        private readonly BirdSettings _birdSettings;
        private readonly ITimeProvider _timeProvider;
        private List<(BirdView, float)> _timeDictionary;

        private List<BirdView> _needRespawn;

        public event Action<BirdView> BirdRespawned;

        public BirdRespawnService(
            BirdSettings birdSettings,
            ITimeProvider timeProvider)
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
                var turple = _timeDictionary.First(x => x.Item1.Equals(birdView));
                _timeDictionary.Remove(turple);

                BirdRespawned?.Invoke(birdView);
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
                }
            }
        }

        public void AddBirdToRespawn(BirdView birdView)
        {
            _timeDictionary.Add((birdView, _birdSettings.RespawnTime));
        }
    }
}