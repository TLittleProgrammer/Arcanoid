using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View.Collisions
{
    public sealed class EntityCollisionsService : IEntityCollisionService, IGeneralRestartable
    {
        private readonly List<IEntityView> _views;
        private readonly IEffectActivator _effectActivator;

        public EntityCollisionsService(IEffectActivator effectActivator)
        {
            _views = new();
            _effectActivator = effectActivator;
        }
        
        public void AddEntity(IEntityView entityView)
        {
            _views.Add(entityView);

            entityView.Colliderable += OnEntityCollidired;
        }

        public void Clear()
        {
            foreach (IEntityView view in _views)
            {
                view.Colliderable -= OnEntityCollidired;
            }
            
            _views.Clear();
        }

        public void Restart()
        {
            Clear();
        }

        private void OnEntityCollidired(IEntityView entity, Collider2D collider)
        {
            _effectActivator.ActivateEffect(entity, collider);
        }
    }
}