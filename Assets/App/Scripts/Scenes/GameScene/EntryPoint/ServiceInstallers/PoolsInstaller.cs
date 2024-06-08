using System;
using System.Collections.Generic;
using App.Scripts.External.Extensions.ZenjectExtensions;
using App.Scripts.External.ObjectPool;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun.Bullets;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Effects.Bombs;
using App.Scripts.Scenes.GameScene.Features.Effects.ObjectPool;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird;
using App.Scripts.Scenes.GameScene.Features.Entities.TopSprites;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;
using App.Scripts.Scenes.GameScene.Features.Pools;
using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class PoolsInstaller : Installer<PoolProviders, EffectsPrefabProvider, PoolsInstaller>
    {
        private readonly PoolProviders _poolProviders;
        private readonly EffectsPrefabProvider _effectsPrefabProvider;

        public PoolsInstaller(PoolProviders poolProviders, EffectsPrefabProvider effectsPrefabProvider)
        {
            _poolProviders = poolProviders;
            _effectsPrefabProvider = effectsPrefabProvider;
        }
        
        public override void InstallBindings()
        {
            BindZenjectPool<EntityView, EntityView.Pool>(PoolTypeId.EntityView);
            BindZenjectPool<HealthPointView, HealthPointView.Pool>(PoolTypeId.HealthPointView);
            BindZenjectPool<OnTopSprites, OnTopSprites.Pool>(PoolTypeId.OnTopSprite);
            BindZenjectPool<BoostView, BoostView.Pool>(PoolTypeId.Boosts);
            BindZenjectPool<BulletView, BulletView.Pool>(PoolTypeId.Bullets);
            BindZenjectPool<BulletEffectView, BulletEffectView.Pool>(PoolTypeId.BulletEffect);
            BindZenjectPool<BallView, BallView.Pool>(PoolTypeId.BallView);
            BindZenjectPool<LaserEffect, LaserEffect.Pool>(PoolTypeId.Laser);
            BindZenjectPool<PlazmaEffect, PlazmaEffect.Pool>(PoolTypeId.Plazma);
            BindZenjectPool<ExplosionEffect, ExplosionEffect.Pool>(PoolTypeId.Explosion);
            BindZenjectPool<BirdView, BirdView.Pool>(PoolTypeId.BirdView);

            BindOwnPool<CircleEffects>();
            BindOwnPool<ExplosionEffect>();

            Container
                .Bind<IKeyObjectPool<IEffect>>()
                .To<KeyObjectPool<IEffect>>()
                .AsSingle()
                .NonLazy();
        }

        private void BindOwnPool<TEffectPool>() where TEffectPool : MonoEffect
        {
            EffectData effectData = _effectsPrefabProvider.Provider[typeof(TEffectPool)];

            Container
                .Bind<IKeyableObjectPool<IEffect>>()
                .To<MonoObjectPool<TEffectPool>>()
                .AsSingle()
                .WithArguments(EffectSpawner<TEffectPool>(effectData), effectData.InitialSize, effectData.PoolParentName, effectData.PoolKey);
        }

        private void BindZenjectPool<TInstance, TPool>(PoolTypeId poolType) where TPool : IMemoryPool where TInstance : MonoBehaviour 
        {
            Container.BindPool<TInstance, TPool>(_poolProviders.Pools[poolType].InitialSize, _poolProviders.Pools[poolType].View.GetComponent<TInstance>(), _poolProviders.Pools[poolType].ParentName);
        }

        private Func<TEffect> EffectSpawner<TEffect>(EffectData effectData)
        {
            return () => Object.Instantiate(effectData.Prefab).GetComponent<TEffect>();
        }
    }
}