using App.Scripts.Scenes.GameScene.Dotween;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public sealed class TweenersLocatorInstaller : Installer<TweenersLocatorInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ITweenersLocator>().To<TweenersLocator>().AsSingle();
        }
    }
}