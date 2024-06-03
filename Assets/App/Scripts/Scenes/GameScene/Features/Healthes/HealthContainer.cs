using System;
using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Constants;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Healthes
{
    public sealed class HealthContainer : IHealthContainer, ILevelProgressSavable, IInitializeByLevelProgress
    {
        private readonly IViewHealthPointService _viewHealthPointService;

        private int _currentHealthCounter;
        private int _maxHealthCounter;

        public HealthContainer(IViewHealthPointService viewHealthPointService)
        {
            _viewHealthPointService = viewHealthPointService;
        }

        public event Action LivesAreWasted;
        public event Action<int> GetDamage;

        public async UniTask AsyncInitialize(int healthes)
        {
            _currentHealthCounter = _maxHealthCounter = healthes == 0 ? GameConstants.DefaultHealthCount : healthes;

            await UniTask.CompletedTask;
        }

        public void UpdateHealth(int healthCount, bool needRestart = true)
        {
            if (_currentHealthCounter + healthCount > _maxHealthCounter)
            {
                SetFullHealthPoints(healthCount);
            }
            else
            {
                ChangeHealthPoints(healthCount, needRestart);
            }    
        }

        private void ChangeHealthPoints(int healthCount, bool needRestart)
        {
            _currentHealthCounter += healthCount;
            if (_currentHealthCounter < -1)
            {
                _currentHealthCounter = -1;
            }

            if (needRestart)
            {
                IsLivesAreWasted();
                IsDamage(healthCount);
            }

            _viewHealthPointService.UpdateHealth(healthCount);
        }

        private void SetFullHealthPoints(int healthCount)
        {
            _currentHealthCounter = _maxHealthCounter;

            _viewHealthPointService.UpdateHealth(_maxHealthCounter - healthCount);
        }

        private void IsLivesAreWasted()
        {
            if(_currentHealthCounter < 0)
            {
                LivesAreWasted?.Invoke();
            }
        }

        private void IsDamage(int healthCount)
        {
            if (healthCount < 0)
            {
                GetDamage?.Invoke(healthCount);
            }
        }

        public void Restart()
        {
            _currentHealthCounter = _maxHealthCounter;
        }

        public void SaveProgress(LevelDataProgress levelDataProgress)
        {
            levelDataProgress.AllHealthes = _maxHealthCounter;
            levelDataProgress.CurrentHealth = _currentHealthCounter;
        }

        public void LoadProgress(LevelDataProgress levelDataProgress)
        {
            _maxHealthCounter = _currentHealthCounter = levelDataProgress.AllHealthes;
        }
    }
}