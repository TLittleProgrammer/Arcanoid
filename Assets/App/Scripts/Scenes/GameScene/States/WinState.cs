using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Constants;
using App.Scripts.General.Energy;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Popup;
using App.Scripts.General.UserData.Energy;
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
        private readonly IEnergyService _energyService;
        private readonly IEnergyDataService _energyDataService;
        private readonly WinViewModel _winViewModel;

        private WinPopupView _winPopupView;

        public WinState(
            ITimeScaleAnimator timeScaleAnimator,
            IPopupService popupService,
            IEnergyService energyService,
            IEnergyDataService energyDataService,
            WinViewModel winViewModel)
        {
            _timeScaleAnimator = timeScaleAnimator;
            _popupService = popupService;
            _energyService = energyService;
            _energyDataService = energyDataService;
            _winViewModel = winViewModel;
        }
        
        public async UniTask Enter()
        {
            await _timeScaleAnimator.Animate(0f);
            
            ShowPopup();
            
            _energyDataService.AddEnergyByPassedLevel();
        }

        public async UniTask Exit()
        {
            _energyService.RemoveView(_winPopupView.EnergyView);
            
            await UniTask.CompletedTask;
        }
        private void ShowPopup()
        {
            _winPopupView = _popupService.Show<WinPopupView>();
            _energyService.AddView(_winPopupView.EnergyView);
            
            _winViewModel.InstallView(_winPopupView);
        }
    }
}