using App.Scripts.External.Extensions.ZenjectExtensions;
using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Effects.Bombs;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird;
using App.Scripts.Scenes.GameScene.Features.Entities.TopSprites;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;
using App.Scripts.Scenes.GameScene.Features.MiniGun;
using App.Scripts.Scenes.GameScene.Features.Pools;
using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class PoolsInstaller : Installer<PoolProviders, PoolsInstaller>
    {
        private readonly PoolProviders _poolProviders;

        public PoolsInstaller(PoolProviders poolProviders)
        {
            _poolProviders = poolProviders;
        }
        
        public override void InstallBindings()
        {
            BindPool<EntityView, EntityView.Pool>(PoolTypeId.EntityView);
            BindPool<CircleEffect, IEffect<CircleEffect>.Pool>(PoolTypeId.CircleEffect);
            BindPool<HealthPointView, HealthPointView.Pool>(PoolTypeId.HealthPointView);
            BindPool<OnTopSprites, OnTopSprites.Pool>(PoolTypeId.OnTopSprite);
            BindPool<BoostView, BoostView.Pool>(PoolTypeId.Boosts);
            BindPool<BulletView, BulletView.Pool>(PoolTypeId.Bullets);
            BindPool<BulletEffectView, BulletEffectView.Pool>(PoolTypeId.BulletEffect);
            BindPool<BallView, BallView.Pool>(PoolTypeId.BallView);
            BindPool<LaserEffect, LaserEffect.Pool>(PoolTypeId.Laser);
            BindPool<PlazmaEffect, PlazmaEffect.Pool>(PoolTypeId.Plazma);
            BindPool<ExplosionEffect, ExplosionEffect.Pool>(PoolTypeId.Explosion);
            BindPool<BirdView, BirdView.Pool>(PoolTypeId.BirdView);
            
            Container.Bind<IPoolContainer>().To<PoolContainer>().AsSingle();
        }

        private void BindPool<TInstance, TPool>(PoolTypeId poolType) where TPool : IMemoryPool where TInstance : MonoBehaviour 
        {
            Container.BindPool<TInstance, TPool>(_poolProviders.Pools[poolType].InitialSize, _poolProviders.Pools[poolType].View.GetComponent<TInstance>(), _poolProviders.Pools[poolType].ParentName);
        }
    }
}