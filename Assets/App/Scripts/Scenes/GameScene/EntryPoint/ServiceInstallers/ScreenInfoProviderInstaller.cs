using App.Scripts.Scenes.GameScene.ScreenInfo;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class ScreenInfoProviderInstaller : Installer<ScreenInfoProviderInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IScreenInfoProvider>().To<ScreenInfoProvider>().AsSingle();
        }
    }
}