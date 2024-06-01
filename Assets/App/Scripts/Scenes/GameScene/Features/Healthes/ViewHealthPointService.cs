using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.Scenes.GameScene.Features.Constants;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Healthes
{
    public sealed class ViewHealthPointService : IViewHealthPointService, IInitializeByLevelProgress
    {
        private readonly List<IHealthPointView> _healthPointViews;
        private readonly IHealthPointView.Factory _healthPointViewFactory;
        private readonly HealthPointView.Pool _healthPointViewPool;
        private readonly ITransformable _parent;

        private int _maxHealthCount;
        private int _currentHealthCount;
        private int _currentHealthIndex;
        private bool _isAnimated;
        private Queue<int> _queriesToChangeHealth;

        public ViewHealthPointService(
            ITransformable parent,
            IHealthPointView.Factory healthPointViewFactory,
            HealthPointView.Pool healthPointViewPool)
        {
            _healthPointViewFactory = healthPointViewFactory;
            _healthPointViewPool = healthPointViewPool;
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

                SetFromAndToByStep(step, out var to, out var from);

                currentHealthOffset = Mathf.Abs(currentHealthOffset);

                for (int i = 0; i < currentHealthOffset; i++)
                {
                    if (step > 0f)
                    {
                        _currentHealthIndex = _currentHealthCount + 1;
                    }
                    else
                    {
                        _currentHealthIndex = _currentHealthCount;
                    }
                    
                    if (_healthPointViews.Count <= _currentHealthIndex || _currentHealthIndex < 0)
                    {
                        break;
                    }
                    
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
            _healthPointViews[_currentHealthIndex].Image.fillAmount = value;
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
            foreach (IHealthPointView pointView in _healthPointViews)
            {
                _healthPointViewPool.Despawn(pointView as HealthPointView);
            }
            
            _healthPointViews.Clear();
            InstallAllViews();
        }

        public void LoadProgress(LevelDataProgress levelDataProgress)
        {
            _maxHealthCount = levelDataProgress.AllHealthes;

            InstallAllViews();
            UpdateHealth(-(_maxHealthCount - levelDataProgress.CurrentHealth));
        }
    }
}