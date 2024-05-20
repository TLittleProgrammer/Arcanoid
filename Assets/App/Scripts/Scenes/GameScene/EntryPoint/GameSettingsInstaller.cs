using App.Scripts.Scenes.GameScene.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Settings;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Installers
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
        }
    }
}