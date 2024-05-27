using App.Scripts.External.Components;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.States;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Popups.Buttons
{
    public abstract class OpenPopupButton : MonoBehaviour, IPointerClickHandler
    {
        private RootUIViewProvider _rootUIViewProvider;
        private IStateMachine _stateMachine;

        [Inject]
        private void Construct(
            RootUIViewProvider rootUIViewProvider,
            IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _rootUIViewProvider = rootUIViewProvider;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _stateMachine.Enter<MenuPopupState, ITransformable>(_rootUIViewProvider.PopupUpViewProvider);
        }
    }
}