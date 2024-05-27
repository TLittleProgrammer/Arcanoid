using App.Scripts.External.Components;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Popups;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States
{
    public class MenuPopupState : IState<ITransformable>
    {
        private readonly IPopupService _popupService;

        public MenuPopupState(IPopupService popupService)
        {
            _popupService = popupService;
        }
        
        public async UniTask Enter(ITransformable transformable)
        {
            _popupService.Show<MenuPopupView>(transformable);
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}