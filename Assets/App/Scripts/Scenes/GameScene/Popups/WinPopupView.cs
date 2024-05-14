using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Constants;
using App.Scripts.Scenes.GameScene.States;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Popups
{
    public class WinPopupView : ViewPopupProvider
    {
        [SerializeField] private Button _continueButton;
        
        private IStateMachine _gameStateMachine;

        [Inject]
        private void Construct(
            [Inject(Id = BindingConstants.GameStateMachine)] IStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void OnEnable()
        {
            _continueButton.onClick.AddListener(Continue);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(Continue);
        }

        public override async UniTask Show()
        {
            await transform.DOScale(Vector3.one, 1f).From(Vector3.zero);
        }

        private void Continue()
        {
            LockButtons();
            _gameStateMachine.Enter<LoadNextLevelState>();
        }

        public override void LockButtons()
        {
            _continueButton.interactable = false;
        }

        public override void UnlockButtons()
        {
        }
    }
}