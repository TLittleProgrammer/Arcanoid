using System;
using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird
{
    public class BirdsHealthContainer : IBirdHealthPointContainer
    {
        private readonly BirdSettings _birdSettings;
        private readonly BirdRespawnService _birdRespawnService;

        private Dictionary<BirdView, int> _birdHealthes;
        
        public BirdsHealthContainer(BirdSettings birdSettings, BirdRespawnService birdRespawnService)
        {
            _birdSettings = birdSettings;
            _birdRespawnService = birdRespawnService;
            _birdHealthes = new();
        }

        public event Action<BirdView> BirdDied;

        public void AddBird(BirdView birdView)
        {
            if (_birdHealthes.ContainsKey(birdView))
            {
                if (_birdHealthes[birdView] <= 0)
                {
                    _birdHealthes[birdView] = _birdSettings.InitialHealthCount;
                    birdView.Collidered += GetDamage;
                }
            }
            else
            {
                _birdHealthes.Add(birdView, _birdSettings.InitialHealthCount);
                birdView.Collidered += GetDamage;
            }
        }

        public void GetDamage(BirdView birdView, Collision2D collision2D)
        {
            if (collision2D.collider.TryGetComponent(out BallView ballView))
            {
                _birdHealthes[birdView]--;

                if (_birdHealthes[birdView] <= 0)
                {
                    birdView.Collidered -= GetDamage;
                    BirdDied?.Invoke(birdView);
                    _birdRespawnService.AddBirdToRespawn(birdView);
                }
            }
        }
    }
}