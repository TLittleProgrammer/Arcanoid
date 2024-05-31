using App.Scripts.Scenes.GameScene.Features.Time;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class TimeProviderInstaller : Installer<TimeProviderInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TimeProvider>().AsSingle();
            Container.Bind<ITimeScaleAnimator>().To<TimeScaleAnimator>().AsSingle();
        }
    }
}