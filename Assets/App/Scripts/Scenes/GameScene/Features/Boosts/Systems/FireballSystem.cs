using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading.Loader;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Systems
{
    public class FireballSystem : IFireballSystem
    {
        private readonly IEntityDestroyable _entityDestroyable;
        private readonly ILevelViewUpdater _levelViewUpdater;
        private readonly IEffectActivator _effectActivator;
        private readonly ILevelLoader _levelLoader;

        public FireballSystem(
            IEntityDestroyable entityDestroyable,
            ILevelViewUpdater levelViewUpdater,
            IEffectActivator effectActivator,
            ILevelLoader levelLoader)
        {
            _entityDestroyable = entityDestroyable;
            _levelViewUpdater = levelViewUpdater;
            _effectActivator = effectActivator;
            _levelLoader = levelLoader;
        }

        public void Activate()
        {
            foreach (IEntityView entityView in _levelLoader.Entities)
            {
                entityView.TriggerColliderable += OnEntityTriggerCollidered;
            }
        }

        public void Disable()
        {
            foreach (IEntityView entityView in _levelLoader.Entities)
            {
                entityView.TriggerColliderable -= OnEntityTriggerCollidered;
            }
        }

        private void OnEntityTriggerCollidered(IEntityView view, Collider2D collider)
        {
            if (collider.TryGetComponent(out BallView ballView))
            {
                if (CanDestroy(view) && ballView.RedBall.gameObject.activeSelf)
                {
                    DestroyView(view, ballView);
                }
            }
        }

        private bool CanDestroy(IEntityView entityView)
        {
            return _levelViewUpdater.LevelGridItemData[entityView.GridPositionX, entityView.GridPositionY].CurrentHealth > 0;
        }

        private void DestroyView(IEntityView view, BallView ballView)
        {
            view.BoxCollider2D.isTrigger = false;
            view.BoxCollider2D.enabled = false;

            GridItemData gridItemData = _levelViewUpdater.LevelGridItemData[view.GridPositionX, view.GridPositionY];
            gridItemData.CurrentHealth = -1;
            
            _effectActivator.ActivateEffect(view, ballView.Collider2D);
            
            _entityDestroyable.Destroy(gridItemData, view);
        }
    }
}