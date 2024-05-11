using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Effects;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Levels.View;
using App.Scripts.Scenes.GameScene.Pools;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Collisions
{
    public sealed class EntityCollisionService : ICollisionService<EntityView>
    {
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IPoolContainer _poolContainer;

        public EntityCollisionService(ILevelViewUpdater levelViewUpdater, IPoolContainer poolContainer)
        {
            _levelViewUpdater = levelViewUpdater;
            _poolContainer = poolContainer;
        }
        
        public void Collide(EntityView entity)
        {
            UpdateGridVisual(entity);
            PlayEffects(entity);
        }

        private void PlayEffects(EntityView entityView)
        {
            CircleEffect circleEffect = _poolContainer.GetItem<CircleEffect>(PoolTypeId.CircleEffect);
            circleEffect.Position = entityView.Position;
            circleEffect.Scale = entityView.Scale.x;
            circleEffect.PlayEffect();
        }

        private void UpdateGridVisual(EntityView entity)
        {
            _levelViewUpdater.UpdateVisual(entity);
        }
    }
}