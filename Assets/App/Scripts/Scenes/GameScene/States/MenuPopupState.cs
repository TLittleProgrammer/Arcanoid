using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Time;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Menu;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.States
{
    public class MenuPopupState : IState<MenuViewModel>
    {
        private readonly IPopupService _popupService;
        private readonly ITimeProvider _timeProvider;

        public MenuPopupState(IPopupService popupService, ITimeProvider timeProvider)
        {
            _popupService = popupService;
            _timeProvider = timeProvider;
        }
        
        public async UniTask Enter(MenuViewModel menuViewModel)
        {
            Time.timeScale = 0f;
            MenuPopupView menuPopupView = _popupService.GetPopup<MenuPopupView>();
            
            menuViewModel.InitializeView(menuPopupView);
            await menuPopupView.Show();
        }

        public async UniTask Exit()
        {
            Time.timeScale = 1f;
            await UniTask.CompletedTask;
        }
    }
}