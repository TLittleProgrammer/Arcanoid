using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Menu;
using App.Scripts.Scenes.GameScene.States;

namespace App.Scripts.Scenes.GameScene.Command
{
    public class OpenMenuPopupCommand : IOpenMenuPopupCommand
    {
        private readonly IStateMachine _stateMachine;
        private readonly MenuViewModel _menuViewModel;

        public OpenMenuPopupCommand(IStateMachine stateMachine, MenuViewModel menuViewModel)
        {
            _stateMachine = stateMachine;
            _menuViewModel = menuViewModel;
        }
        
        public void Execute()
        {
            _stateMachine.Enter<MenuPopupState, MenuViewModel>(_menuViewModel);
        }
    }
}