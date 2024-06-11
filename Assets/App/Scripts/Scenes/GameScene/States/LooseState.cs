using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Time;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Loose;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States
{
    public class LooseState : IState
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IPopupService _popupService;
        private readonly LooseViewModel _looseViewModel;

        public LooseState(
            ITimeProvider timeProvider,
            IPopupService popupService,
            LooseViewModel looseViewModel)
        {
            _timeProvider = timeProvider;
            _popupService = popupService;
            _looseViewModel = looseViewModel;
        }
        
        public async UniTask Enter()
        {
            _timeProvider.TimeScale = 0f;
            
            LoosePopupView loosePopupView = _popupService.GetPopup<LoosePopupView>();
            _looseViewModel.InstallView(loosePopupView);
            
            await loosePopupView.Show();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}