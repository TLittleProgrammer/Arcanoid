using App.Scripts.External.GameStateMachine;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Levels;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Popups;
using App.Scripts.Scenes.GameScene.Time;

namespace App.Scripts.Scenes.GameScene.States
{
    public class WinState : IState
    {
        private readonly ITimeScaleAnimator _timeScaleAnimator;
        private readonly IPopupService _popupService;
        private readonly RootUIViewProvider _rootUIViewProvider;
        private readonly IStateMachine _gameStateMachine;
        private readonly ILevelPackInfoService _levelPackInfoService;

        private WinPopupView _winPopupView;

        public WinState(
            ITimeScaleAnimator timeScaleAnimator,
            IPopupService popupService,
            RootUIViewProvider rootUIViewProvider,
            IStateMachine gameStateMachine,
            ILevelPackInfoService levelPackInfoService)
        {
            _popupService = popupService;
            _timeScaleAnimator = timeScaleAnimator;
            _popupService = popupService;
            _rootUIViewProvider = rootUIViewProvider;
            _gameStateMachine = gameStateMachine;
            _levelPackInfoService = levelPackInfoService;
        }
        
        public async void Enter()
        {
            await _timeScaleAnimator.Animate(0f);
            ShowPopup();
            Subscribe();
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            LevelPack currentPack = _levelPackInfoService.GetData().LevelPack;
            _winPopupView.GalacticIcon.sprite = currentPack.GalacticIcon;
            _winPopupView.GalacticName.SetToken(currentPack.LocaleKey);
            _winPopupView.PassedLevelsText.text = $"{_levelPackInfoService.GetData().LevelIndex + 1}/{currentPack.Levels.Count}";
        }

        private void Subscribe()
        {
            _winPopupView.ContinueButton.onClick.AddListener(OnContinueClicked);
        }

        private void ShowPopup()
        {
            _winPopupView = (WinPopupView)_popupService.Show<WinPopupView>(_rootUIViewProvider.PopupUpViewProvider);
        }

        public void Exit()
        {
            _winPopupView.ContinueButton.onClick.RemoveListener(OnContinueClicked);
        }

        private void OnContinueClicked()
        {
            _winPopupView.ContinueButton.interactable = false;
            _gameStateMachine.Enter<LoadNextLevelState>();
        }
    }
}