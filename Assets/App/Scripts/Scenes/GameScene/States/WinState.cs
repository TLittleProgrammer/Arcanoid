using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;

namespace App.Scripts.Scenes.GameScene.States
{
    public class WinState : IState
    {
        private readonly IPopupService _popupService;

        public WinState(IPopupService popupService)
        {
            _popupService = popupService;
        }
        
        public void Enter()
        {
        }

        public void Exit()
        {
            
        }
    }
}