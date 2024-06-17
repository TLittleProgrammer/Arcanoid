using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Command;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.Popup;
using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.GameScene.Command.Win;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Time;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Win;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States
{
    public class WinState : IState
    {
        private readonly ITimeScaleAnimator _timeScaleAnimator;
        private readonly IPopupService _popupService;
        private readonly IEnergyDataService _energyDataService;
        private readonly WinViewModel _winViewModel;
        private readonly EnergyViewModel _energyViewModel;
        private readonly IDisableButtonsCommand _disableButtonsCommand;
        private readonly ILoadNextLevelCommand _loadNextLevelCommand;
        private readonly ITweenersLocator _tweenersLocator;

        private WinPopupView _winPopupView;

        public WinState(
            ITimeScaleAnimator timeScaleAnimator,
            IPopupService popupService,
            IEnergyDataService energyDataService,
            WinViewModel winViewModel,
            EnergyViewModel energyViewModel,
            IDisableButtonsCommand disableButtonsCommand,
            ILoadNextLevelCommand loadNextLevelCommand,
            ITweenersLocator tweenersLocator)
        {
            _timeScaleAnimator = timeScaleAnimator;
            _popupService = popupService;
            _energyDataService = energyDataService;
            _winViewModel = winViewModel;
            _energyViewModel = energyViewModel;
            _disableButtonsCommand = disableButtonsCommand;
            _loadNextLevelCommand = loadNextLevelCommand;
            _tweenersLocator = tweenersLocator;
        }
        
        public async UniTask Enter()
        {
            await _timeScaleAnimator.Animate(0f);
            
            ShowPopup();
            
            _energyDataService.AddEnergyByPassedLevel();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
        
        private void ShowPopup()
        {
            _winPopupView = _popupService.GetPopup<WinPopupView>();
            
            _winPopupView.Initialize(_winViewModel, _disableButtonsCommand, _loadNextLevelCommand, _tweenersLocator, _energyViewModel);

            _winPopupView.Show().Forget();
        }
    }
}