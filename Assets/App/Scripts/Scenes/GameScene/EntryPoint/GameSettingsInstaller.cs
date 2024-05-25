using App.Scripts.Scenes.GameScene.Features.Levels.AssetManagement;
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
        }
    }
}