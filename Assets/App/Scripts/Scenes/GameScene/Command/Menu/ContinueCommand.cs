using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.States;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Command
{
    public class ContinueCommand : IContinueCommand
    {
        private readonly IPopupService _popupService;
        private readonly IStateMachine _stateMachine;

        public ContinueCommand(IPopupService popupService, IStateMachine stateMachine)
        {
            _popupService = popupService;
            _stateMachine = stateMachine;
        }
        
        public async void Execute()
        {
            await _popupService.CloseAll();
            
            _stateMachine.Enter<GameLoopState>().Forget();
        }
    }
}