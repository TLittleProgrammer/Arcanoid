using System.Collections.Generic;
using App.Scripts.External.ObjectPool;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.General.View;
using App.Scripts.Scenes.GameScene.Features.Settings;

namespace App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices.BombDestroyers
{
    public abstract class BombDestroyer : DestroyService
    {
        private readonly DestroyEntityEffectMapping _destroyEntityEffectMapping;
        private readonly IKeyObjectPool<IEffect> _keyObjectPool;

        protected BombDestroyer(
            ILevelViewUpdater levelViewUpdater,
            DestroyEntityEffectMapping destroyEntityEffectMapping,
            IKeyObjectPool<IEffect> keyObjectPool) : base(levelViewUpdater)
        {
            _destroyEntityEffectMapping = destroyEntityEffectMapping;
            _keyObjectPool = keyObjectPool;
        }
        
        private Dictionary<string, List<string>> EffectsByIdMapping => _destroyEntityEffectMapping.DestroyEffectsMapping;

        protected void SetExplosionsEffect(IEntityView entityView)
        {
            string id = entityView.EntityId.ToString();

            if (EffectsByIdMapping.ContainsKey(id))
            {
                foreach (string effectName in EffectsByIdMapping[id])
                {
                    IEffect effect = _keyObjectPool.Spawn(effectName);
                    
                    effect.PlayEffect(entityView.GameObject.transform, entityView.GameObject.transform);
                    effect.Disabled += needDisableEffect =>
                    {
                        OnEffectDisabled(needDisableEffect, effectName);
                    };
                }
            }
        }

        private void OnEffectDisabled(IEffect effect, string effectName)
        {
            _keyObjectPool.Despawn(effectName, effect);
        }

        public abstract override void Destroy(GridItemData gridItemData, IEntityView entityView);
    }
}