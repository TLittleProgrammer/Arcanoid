using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Time;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States
{
    public class LooseState : IState
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IPopupService _popupService;

        public LooseState(ITimeProvider timeProvider, IPopupService popupService)
        {
            _timeProvider = timeProvider;
            _popupService = popupService;
        }
        
        public async UniTask Enter()
        {
            _timeProvider.TimeScale = 0f;
            _popupService.Show<LoosePopupView>();

            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}