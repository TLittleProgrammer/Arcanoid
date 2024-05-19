using System;
using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Constants;
using App.Scripts.Scenes.GameScene.Levels;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Healthes
{
    public sealed class HealthContainer : IHealthContainer
    {
        private readonly IHealthPointService _healthPointService;

        private IEnumerable<IRestartable> _restartables;
        private int _currentHealthCounter;
        private int _maxHealthCounter;

        public HealthContainer(IHealthPointService healthPointService)
        {
            _healthPointService = healthPointService;
        }

        public event Action LivesAreWasted;

        public async UniTask AsyncInitialize(LevelData param, IEnumerable<IRestartable> restartables)
        {
            _currentHealthCounter = _maxHealthCounter = param.HealthCount == 0 ? GameConstants.DefaultHealthCount : param.HealthCount;
            _restartables = restartables;
            
            await UniTask.CompletedTask;
        }

        public void UpdateHealth(int healthCount)
        {
            _healthPointService.UpdateHealth(healthCount);

            UpdateHealthCounter(healthCount);
            RestartServicesIfNeed(healthCount);
        }

        private void UpdateHealthCounter(int healthCount)
        {
            _currentHealthCounter += healthCount;

            if (_currentHealthCounter < 0)
            {
                LivesAreWasted?.Invoke();
            }
        }

        private void RestartServicesIfNeed(int healthCount)
        {
            if (healthCount < 0)
            {
                foreach (IRestartable restartable in _restartables)
                {
                    restartable.Restart();
                }
            }
        }

        public void Restart()
        {
            _currentHealthCounter = _maxHealthCounter;
        }
    }
}