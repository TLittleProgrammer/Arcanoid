using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.States;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Popups.Buttons
{
    public abstract class OpenPopupButton : MonoBehaviour, IPointerClickHandler
    {
        private RootUIViewProvider _rootUIViewProvider;
        private IPopupService _popupService;
        private IStateMachine _stateMachine;

        [Inject]
        private void Construct(
            RootUIViewProvider rootUIViewProvider,
            IPopupService popupService,
            IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _popupService = popupService;
            _rootUIViewProvider = rootUIViewProvider;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _popupService.Show<MenuPopupView>(_rootUIViewProvider.PopupUpViewProvider);
            _stateMachine.Enter<PopupState>();
        }
    }
}