using App.Scripts.External.Components;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;
using App.Scripts.Scenes.GameScene.Features.Pools;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Factories.Health
{
    public class HealthFactory : IFactory<ITransformable, IHealthPointView>
    {
        private readonly IPoolContainer _poolContainer;

        public HealthFactory(IPoolContainer poolContainer)
        {
            _poolContainer = poolContainer;
        }

        public IHealthPointView Create(ITransformable targetType)
        {
            HealthPointView healthView = _poolContainer.GetItem<HealthPointView>(PoolTypeId.HealthPointView);

            healthView.Image.fillAmount = 1f;
            
            Transform healthTransform = healthView.transform;
            healthTransform.SetParent(targetType.Transform, false);
            healthTransform.localScale = Vector3.one;

            return healthView;
        }
    }
}