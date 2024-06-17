using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Command;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.Popup;
using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Time;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Loose;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.States
{
    public class LooseState : IState
    {
        private readonly ITimeProvider _timeProvider;
        private readonly IPopupService _popupService;
        private readonly LooseViewModel _looseViewModel;
        private readonly EnergyViewModel _energyViewModel;
        private readonly IEnergyDataService _energyDataService;
        private readonly IRestartCommand _restartCommand;
        private readonly IBackCommand _backCommand;
        private readonly IDisableButtonsCommand _disableButtonsCommand;
        private readonly IBuyHealthCommand _buyHealthCommand;

        private float _previousTimeScale;
        
        public LooseState(
            ITimeProvider timeProvider,
            IPopupService popupService,
            LooseViewModel looseViewModel,
            EnergyViewModel energyViewModel,
            IEnergyDataService energyDataService,
            IRestartCommand restartCommand,
            IBackCommand backCommand,
            IDisableButtonsCommand disableButtonsCommand,
            IBuyHealthCommand buyHealthCommand)
        {
            _timeProvider = timeProvider;
            _popupService = popupService;
            _looseViewModel = looseViewModel;
            _energyViewModel = energyViewModel;
            _energyDataService = energyDataService;
            _restartCommand = restartCommand;
            _backCommand = backCommand;
            _disableButtonsCommand = disableButtonsCommand;
            _buyHealthCommand = buyHealthCommand;
        }

        public UniTask Enter()
        {
            _previousTimeScale = _timeProvider.TimeScale;
            _timeProvider.TimeScale = 0f;
            Time.timeScale = 0f;
            
            LoosePopupView loosePopupView = _popupService.GetPopup<LoosePopupView>();
            
            loosePopupView.SetUp(
                _looseViewModel,
                _energyViewModel,
                _energyDataService,
                _restartCommand,
                _backCommand,
                _disableButtonsCommand,
                _buyHealthCommand);
            
            return loosePopupView.Show();
        }

        public async UniTask Exit()
        {
            Time.timeScale = 1f;
            _timeProvider.TimeScale = _previousTimeScale;
            await UniTask.CompletedTask;
        }
    }
}