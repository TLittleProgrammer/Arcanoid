using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Constants;
using App.Scripts.Scenes.GameScene.States;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Popups.Buttons
{
    public abstract class OpenPopupButton : MonoBehaviour, IPointerClickHandler
    {
        public PopupTypeId NeedOpenPopupType;
        
        private RootUIViewProvider _rootUIViewProvider;
        private IPopupService _popupService;
        private IStateMachine _stateMachine;

        [Inject]
        private void Construct(
            RootUIViewProvider rootUIViewProvider,
            IPopupService popupService,
            [Inject(Id = BindingConstants.GameStateMachine)] IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _popupService = popupService;
            _rootUIViewProvider = rootUIViewProvider;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_stateMachine.CurrentState is GameLoopState)
            {
                _popupService.Show(NeedOpenPopupType, _rootUIViewProvider.PopupUpViewProvider);
                _stateMachine.Enter<PopupState>();
            }
        }
    }
}