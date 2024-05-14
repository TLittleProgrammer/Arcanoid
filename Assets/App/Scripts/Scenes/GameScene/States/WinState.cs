using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Time;

namespace App.Scripts.Scenes.GameScene.States
{
    public class WinState : IState
    {
        private readonly ITimeScaleAnimator _timeScaleAnimator;
        private readonly IPopupService _popupService;
        private readonly RootUIViewProvider _rootUIViewProvider;

        public WinState(
            ITimeScaleAnimator timeScaleAnimator,
            IPopupService popupService,
            RootUIViewProvider rootUIViewProvider)
        {
            _popupService = popupService;
            _timeScaleAnimator = timeScaleAnimator;
            _popupService = popupService;
            _rootUIViewProvider = rootUIViewProvider;
        }
        
        public async void Enter()
        {
            await _timeScaleAnimator.Animate(0f);
            _popupService.Show(PopupTypeId.Win, _rootUIViewProvider.PopupUpViewProvider);
        }

        public void Exit()
        {
            
        }
    }
}