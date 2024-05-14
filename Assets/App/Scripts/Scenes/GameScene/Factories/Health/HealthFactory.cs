using App.Scripts.General.Components;
using App.Scripts.Scenes.GameScene.Healthes.View;
using App.Scripts.Scenes.GameScene.Pools;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Factories.Health
{
    public class HealthFactory : IFactory<ITransformable, IHealthPointView>
    {
        private readonly IPoolContainer _poolContainer;

        public HealthFactory(IPoolContainer poolContainer)
        {
            _poolContainer = poolContainer;
        }

        public IHealthPointView Create(ITransformable parent)
        {
            HealthPointView healthView = _poolContainer.GetItem<HealthPointView>(PoolTypeId.HealthPointView);

            healthView.Image.fillAmount = 1f;
            healthView.transform.SetParent(parent.Transform, false);
            healthView.transform.localScale = Vector3.one;

            return healthView;
        }
    }
}