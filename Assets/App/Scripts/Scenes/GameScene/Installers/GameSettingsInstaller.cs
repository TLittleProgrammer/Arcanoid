using App.Scripts.Scenes.GameScene.Levels.AssetManagement;
using App.Scripts.Scenes.GameScene.Settings;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Installers
{
    [CreateAssetMenu(menuName = "Configs/Settings/Game Settings", fileName = "GameSettings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller
    {
        public FactorySettings FactorySettings;
        public EntityProvider EntityProvider;
        public BallFlyingSettings BallFlyingSettings;

        public override void InstallBindings()
        {
            Container.Bind<FactorySettings>().FromInstance(FactorySettings).IfNotBound();
            Container.Bind<EntityProvider>().FromInstance(EntityProvider).IfNotBound();
            Container.Bind<BallFlyingSettings>().FromInstance(BallFlyingSettings).IfNotBound();
        }
    }
}