using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Menu;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States
{
    public class MenuPopupState : IState<MenuViewModel>
    {
        private readonly IPopupService _popupService;

        public MenuPopupState(IPopupService popupService)
        {
            _popupService = popupService;
        }
        
        public async UniTask Enter(MenuViewModel menuViewModel)
        {
            MenuPopupView menuPopupView = _popupService.GetPopup<MenuPopupView>();
            
            menuViewModel.InitializeView(menuPopupView);
            await menuPopupView.Show();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}