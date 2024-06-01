using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Command;
using App.Scripts.Scenes.GameScene.Features.Popups.Buttons;
using App.Scripts.Scenes.GameScene.MVVM.Main;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Loose;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Menu;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Win;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class MVVMInstaller : Installer<OpenMenuPopupButton, MVVMInstaller>
    {
        private readonly OpenMenuPopupButton _openMenuPopupButton;

        public MVVMInstaller(OpenMenuPopupButton openMenuPopupButton)
        {
            _openMenuPopupButton = openMenuPopupButton;
        }

        public override void InstallBindings()
        {
            Container.Bind<IOpenMenuPopupCommand>().To<OpenMenuPopupCommand>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainViewModel>().AsSingle().WithArguments(_openMenuPopupButton);
            Container.BindInterfacesAndSelfTo<MenuViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<LooseViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<WinViewModel>().AsSingle();
        }
    }
}