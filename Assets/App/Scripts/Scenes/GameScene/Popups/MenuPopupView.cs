using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.States;
using App.Scripts.Scenes.GameScene.Constants;
using App.Scripts.Scenes.GameScene.States;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Popups
{
    public class MenuPopupView : ViewPopupProvider
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _continueButton;

        private Sequence _sequence;
        private IPopupService _popupService;
        private IStateMachine _gameStateMachine;
        private IStateMachine _projectStateMachine;

        [Inject]
        private void Construct(
            IPopupService popupService,
            [Inject(Id = BindingConstants.ProjectStateMachine)] IStateMachine projectStateMachine,
            [Inject(Id = BindingConstants.GameStateMachine)] IStateMachine gameStateMachine)
        {
            _projectStateMachine = projectStateMachine;
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
            _gameStateMachine.Enter<RestartState>();
        }

        private void Back()
        {
            Close().Forget();
            _projectStateMachine.Enter<LoadingSceneState, string, bool>("1.MainMenu", false);
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
            
            await _sequence
                .Append(_continueButton.transform.DOScale(Vector3.zero, 0.25f).From(Vector3.one).SetEase(Ease.OutBounce))
                .Append(_backButton.transform.DOScale(Vector3.zero, 0.25f).From(Vector3.one).SetEase(Ease.OutBounce))
                .Append(_restartButton.transform.DOScale(Vector3.zero, 0.25f).From(Vector3.one).SetEase(Ease.OutBounce))
                .Append(transform.DOScale(Vector3.zero, 1f).From(Vector3.one)).ToUniTask();
        }

        public override void LockButtons()
        {
            
        }

        public override void UnlockButtons()
        {
            
        }
    }
}