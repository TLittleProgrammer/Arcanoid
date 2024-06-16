using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Input;
using App.Scripts.Scenes.GameScene.States.Gameloop;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class InputInstaller : Installer<InputInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ClickDetector>().AsSingle().WhenInjectedInto(typeof(GameLoopState), typeof(InputService), typeof(BallsService));
            Container.BindInterfacesTo<InputService>().AsSingle().WhenNotInjectedInto<TickableManager>();
        }
    }
}