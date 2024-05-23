using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Constants;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Restart;
using App.Scripts.Scenes.GameScene.Features.States;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Popups
{
    public class MenuPopupView : PopupView
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _continueButton;

        private Sequence _sequence;
        private IPopupService _popupService;
        private IStateMachine _gameStateMachine;
        private IRestartService _restartService;
        private ITweenersLocator _tweenersLocator;

        [Inject]
        private void Construct(
            IPopupService popupService,
            IStateMachine gameStateMachine,
            IRestartService restartService,
            ITweenersLocator tweenersLocator)
        {
            _tweenersLocator = tweenersLocator;
            _restartService = restartService;
            _gameStateMachine = gameStateMachine;
            _popupService = popupService;
        }

        private void OnEnable()
        {
            _continueButton.onClick.AddListener(Continue);
            _backButton.onClick.AddListener(Back);
            _restartButton.onClick.AddListener(Restart);
        }

        private void OnDisable()
        {
            _continueButton.onClick.RemoveListener(Continue);
            _backButton.onClick.RemoveListener(Back);
            _restartButton.onClick.RemoveListener(Restart);
        }

        private void Restart()
        {
            _restartService.TryRestart();
        }

        private void Back()
        {
            Close().Forget();
            _gameStateMachine.Enter<LoadSceneFromMainMenuState, string>(SceneNaming.MainMenu);
        }

        private async void Continue()
        {
            await _popupService.Close<MenuPopupView>();
            _gameStateMachine.Enter<GameLoopState>();
        }

        public override UniTask Show()
        {
            _sequence = DOTween.Sequence();
            _sequence
                .Append(transform.DOScale(Vector3.one, 1f).From(Vector3.zero))
                .Append(_restartButton.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce))
                .Append(_backButton.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce))
                .Append(_continueButton.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce));
            
            return UniTask.CompletedTask;
        }

        public override async UniTask Close()
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence();
            
            _tweenersLocator.AddSequence(_sequence
                .Append(_continueButton.transform.DOScale(Vector3.zero, 0.25f).From(Vector3.one).SetEase(Ease.OutBounce))
                .Append(_backButton.transform.DOScale(Vector3.zero, 0.25f).From(Vector3.one).SetEase(Ease.OutBounce))
                .Append(_restartButton.transform.DOScale(Vector3.zero, 0.25f).From(Vector3.one).SetEase(Ease.OutBounce))
                .Append(transform.DOScale(Vector3.zero, 1f).From(Vector3.one)));

            await UniTask.CompletedTask;
        }
    }
}