using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities.AssetManagement;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.Entities.View.Collisions;
using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint
{
    [CreateAssetMenu(menuName = "Configs/Settings/Game Settings", fileName = "GameSettings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller
    {
        public PoolProviders PoolProviders;
        public EntityProvider EntityProvider;
        public BallFlyingSettings BallFlyingSettings;
        public ScoreAnimationSettings ScoreAnimationSettings;
        public CircleWinEffectSettings CircleWinEffectSettings;
        public StopGameSettings StopGameSettings;
        public WinContinueButtonAnimationSettings WinContinueButtonAnimationSettings;
        public BoxCollidersAroundScreenSettings BoxCollidersAroundScreenSettings;
        public BoostViewProvider BoostViewProvider;
        public BoostsSettings BoostsSettings;
        public BirdSettings BirdSettings;
        public ShapeMoverSettings ShapeMoverSettings;
        public EffectCollisionProvider EffectCollisionProvider;
        public EffectsPrefabProvider EffectsPrefabProvider;
        public DestroyEntityEffectMapping DestroyEntityEffectMapping;

        public override void InstallBindings()
        {
            Container.Bind<PoolProviders>().FromInstance(PoolProviders).IfNotBound();
            Container.Bind<EntityProvider>().FromInstance(EntityProvider).IfNotBound();
            Container.Bind<BallFlyingSettings>().FromInstance(BallFlyingSettings).IfNotBound();
            Container.Bind<ScoreAnimationSettings>().FromInstance(ScoreAnimationSettings).IfNotBound();
            Container.Bind<CircleWinEffectSettings>().FromInstance(CircleWinEffectSettings).IfNotBound();
            Container.Bind<StopGameSettings>().FromInstance(StopGameSettings).IfNotBound();
            Container.Bind<WinContinueButtonAnimationSettings>().FromInstance(WinContinueButtonAnimationSettings).IfNotBound();
            Container.Bind<BoxCollidersAroundScreenSettings>().FromInstance(BoxCollidersAroundScreenSettings).IfNotBound();
            Container.Bind<BoostViewProvider>().FromInstance(BoostViewProvider).IfNotBound();
            Container.Bind<BoostsSettings>().FromInstance(BoostsSettings).IfNotBound();
            Container.Bind<BirdSettings>().FromInstance(BirdSettings).IfNotBound();
            Container.Bind<ShapeMoverSettings>().FromInstance(ShapeMoverSettings).IfNotBound();
            Container.Bind<EffectCollisionProvider>().FromInstance(EffectCollisionProvider).IfNotBound();
            Container.Bind<EffectsPrefabProvider>().FromInstance(EffectsPrefabProvider).IfNotBound();
            Container.Bind<DestroyEntityEffectMapping>().FromInstance(DestroyEntityEffectMapping).IfNotBound();
        }
    }
}