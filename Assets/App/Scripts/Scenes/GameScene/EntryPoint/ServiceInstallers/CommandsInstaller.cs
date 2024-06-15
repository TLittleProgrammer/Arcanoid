using App.Scripts.General.Command;
using App.Scripts.Scenes.GameScene.Command;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.Command.Menu;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class CommandsInstaller : Installer<CommandsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DisableButtonsCommand>().AsSingle();
            Container.BindInterfacesAndSelfTo<RestartCommand>().AsSingle();
            Container.BindInterfacesAndSelfTo<SkipLevelCommand>().AsSingle();
        }
    }
}