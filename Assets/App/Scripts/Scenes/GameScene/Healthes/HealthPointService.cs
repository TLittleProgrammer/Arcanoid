using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.Scenes.GameScene.Constants;
using App.Scripts.Scenes.GameScene.Healthes.View;
using App.Scripts.Scenes.GameScene.Levels;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Healthes
{
    public sealed class HealthPointService : IHealthPointService
    {
        private readonly List<IHealthPointView> _healthPointViews;
        private readonly IHealthPointView.Factory _healthPointViewFactory;
        private readonly ITransformable _parent;

        private int _maxHealthCount;
        private int _currentHealthCount;
        private bool _isAnimated;
        private Queue<int> _queriesToChangeHealth;

        public HealthPointService(ITransformable parent, IHealthPointView.Factory healthPointViewFactory)
        {
            _healthPointViewFactory = healthPointViewFactory;
            _parent = parent;
            _healthPointViews = new();
            _queriesToChangeHealth = new();
        }
        
        public async UniTask AsyncInitialize(LevelData param)
        {
            _maxHealthCount = param.HealthCount == 0 ? GameConstants.DefaultHealthCount : param.HealthCount;

            InstallAllViews();
            
            await UniTask.CompletedTask;
        }

        public void UpdateHealth(int healthCount)
        {
            if (_currentHealthCount + healthCount < -1)
                return;
            
            _queriesToChangeHealth.Enqueue(healthCount);
            
            if (!_isAnimated)
            {
                Animate();
            }
        }

        private async void Animate()
        {
            _isAnimated = true;

            while (_queriesToChangeHealth.Count != 0)
            {
                int currentHealthOffset = _queriesToChangeHealth.Dequeue();
                int step = currentHealthOffset > 0 ? 1 : -1;

                float from;
                float to;

                SetFromAndToByStep(step, out to, out from);

                currentHealthOffset = Mathf.Abs(currentHealthOffset);

                for (int i = 0; i < currentHealthOffset; i++)
                {
                    await DOVirtual.Float(from, to, 0.25f, UpdateCurrentHealthImage).ToUniTask();

                    _currentHealthCount += step;
                }
            }

            _isAnimated = false;
        }

        private void SetFromAndToByStep(int step, out float to, out float from)
        {
            if (step < 0)
            {
                from = 1f;
                to = 0f;
            }
            else
            {
                from = 0f;
                to = 1f;
            }
        }

        private void UpdateCurrentHealthImage(float value)
        {
            _healthPointViews[_currentHealthCount].Image.fillAmount = value;
        }

        private void InstallAllViews()
        {
            for (int i = 0; i < _maxHealthCount; i++)
            {
                _healthPointViews.Add(_healthPointViewFactory.Create(_parent));
            }

            _currentHealthCount = _maxHealthCount - 1;
        }

        public void Restart()
        {
            _healthPointViews.Clear();
            InstallAllViews();
        }
    }
}