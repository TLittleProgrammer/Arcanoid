using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.Data;
using App.Scripts.Scenes.GameScene.Features.Levels.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.DestroyServices.BombDestroyers
{
    public abstract class BombDestroyer : DestroyService
    {
        private readonly ExplosionEffect.Pool _explosionsEffectPool;
        public abstract override void Destroy(GridItemData gridItemData, IEntityView entityView);

        protected BombDestroyer(
            ILevelViewUpdater levelViewUpdater,
            ExplosionEffect.Pool explosionsEffectPool) : base(levelViewUpdater)
        {
            _explosionsEffectPool = explosionsEffectPool;
        }

        protected void SetExplosionsEffect(IEntityView entityView)
        {
            float bombSize = entityView.GameObject.transform.localScale.x;
            
            ExplosionEffect effect = _explosionsEffectPool.Spawn();
            ParticleSystem.MainModule explosionMain = effect.Explosion.main;
            explosionMain.startSizeX = bombSize * 1.431f;

            effect.transform.position = entityView.Position;
            effect.Explosion.Play();
        }
    }
}