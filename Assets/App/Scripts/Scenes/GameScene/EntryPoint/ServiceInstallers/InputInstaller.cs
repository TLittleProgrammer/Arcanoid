using App.Scripts.Scenes.GameScene.Input;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class InputInstaller : Installer<InputInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IClickDetector>().To<ClickDetector>().AsSingle();
            Container.Bind<IInputService>().To<InputService>().AsSingle();
        }
    }
}