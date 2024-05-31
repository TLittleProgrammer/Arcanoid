using App.Scripts.Scenes.GameScene.Command;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class CommandsInstaller : Installer<CommandsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DisableButtonsCommand>().AsSingle();
            Container.BindInterfacesAndSelfTo<ContinueCommand>().AsCached().Lazy();
            Container.BindInterfacesAndSelfTo<BackCommand>().AsSingle();
            Container.BindInterfacesAndSelfTo<RestartCommand>().AsSingle();
        }
    }
}