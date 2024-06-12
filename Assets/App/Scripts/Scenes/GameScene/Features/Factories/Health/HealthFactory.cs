using App.Scripts.External.Components;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.Health
{
    public class HealthFactory : IFactory<ITransformable, IHealthPointView>
    {
        private readonly HealthPointView.Pool _healthPointViewPool;

        public HealthFactory(HealthPointView.Pool healthPointViewPool)
        {
            _healthPointViewPool = healthPointViewPool;
        }

        public IHealthPointView Create(ITransformable boostId)
        {
            HealthPointView healthView = _healthPointViewPool.Spawn();

            healthView.Image.fillAmount = 1f;
            
            Transform healthTransform = healthView.transform;
            healthTransform.SetParent(boostId.Transform, false);
            healthTransform.localScale = Vector3.one;

            return healthView;
        }
    }
}