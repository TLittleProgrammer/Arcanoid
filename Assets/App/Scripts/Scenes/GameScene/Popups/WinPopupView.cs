using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Constants;
using App.Scripts.Scenes.GameScene.Dotween;
using App.Scripts.Scenes.GameScene.Settings;
using App.Scripts.Scenes.GameScene.States;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
using Button = UnityEngine.UI.Button;

namespace App.Scripts.Scenes.GameScene.Popups
{
    public class WinPopupView : ViewPopupProvider
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private RectTransform _textFalling;
        [SerializeField] private RectTransform _targetPositionForTextFalling;
        
        private IStateMachine _gameStateMachine;
        private WinContinueButtonAnimationSettings _winContinueButtonAnimationSettings;
        private ITweenersLocator _tweenersLocator;

        [Inject]
        private void Construct(
            [Inject(Id = BindingConstants.GameStateMachine)] IStateMachine gameStateMachine,
            WinContinueButtonAnimationSettings winContinueButtonAnimationSettings,
            ITweenersLocator tweenersLocator)
        {
            _tweenersLocator = tweenersLocator;
            _winContinueButtonAnimationSettings = winContinueButtonAnimationSettings;
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

            _textFalling.DOMoveY(_targetPositionForTextFalling.transform.position.y, 0.5f).SetEase(Ease.OutBounce).ToUniTask().Forget();

            Sequence sequence = DOTween.Sequence();

            sequence.Append(
                _continueButton
                    .transform
                    .DOScale(_winContinueButtonAnimationSettings.TargetScale,
                        _winContinueButtonAnimationSettings.Duration)
                    .SetDelay(_winContinueButtonAnimationSettings.Delay)
                    .SetLoops(2, LoopType.Yoyo))
                .SetEase(_winContinueButtonAnimationSettings.Ease)
                .SetLoops(-1, LoopType.Restart)
                .ToUniTask().Forget();


            _tweenersLocator.AddSequence(sequence);
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