using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Dotween;
using App.Scripts.Scenes.GameScene.Restart;
using App.Scripts.Scenes.GameScene.Settings;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Popups
{
    public class LoosePopupView : PopupView
    {
        [SerializeField] private Button _restartButton;
        
        private ITweenersLocator _tweenersLocator;
        private WinContinueButtonAnimationSettings _popupButtonAnimationSettings;
        private IRestartService _restartService;

        [Inject]
        private void Construct(
            IStateMachine gameStateMachine,
            ITweenersLocator tweenersLocator,
            WinContinueButtonAnimationSettings winContinueButtonAnimationSettings,
            IRestartService restartService)
        {
            _restartService = restartService;
            _popupButtonAnimationSettings = winContinueButtonAnimationSettings;
            _tweenersLocator = tweenersLocator;
        }

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(Restart);
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(Restart);
        }

        public override async UniTask Show()
        {
            await base.Show();
            
            Sequence sequence = DOTween.Sequence();

            //TODO дублированный код, исправить
            sequence.Append(
                    _restartButton
                        .transform
                        .DOScale(_popupButtonAnimationSettings.TargetScale,
                            _popupButtonAnimationSettings.Duration)
                        .SetDelay(_popupButtonAnimationSettings.Delay)
                        .SetLoops(2, LoopType.Yoyo))
                .SetEase(_popupButtonAnimationSettings.Ease)
                .SetLoops(-1, LoopType.Restart)
                .ToUniTask().Forget();


            _tweenersLocator.AddSequence(sequence);
        }

        private void Restart()
        {
            _restartService.TryRestart();
        }
    }
}