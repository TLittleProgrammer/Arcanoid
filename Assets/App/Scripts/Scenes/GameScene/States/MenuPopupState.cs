using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Command;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.Popup;
using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Time;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Menu;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.States
{
    public class MenuPopupState : IState<MenuViewModel>
    {
        private readonly IPopupService _popupService;
        private readonly MenuViewModel _menuViewModel;
        private readonly IRestartCommand _restartCommand;
        private readonly IBackCommand _backCommand;
        private readonly IContinueCommand _continueCommand;
        private readonly IDisableButtonsCommand _disableButtonsCommand;
        private readonly EnergyViewModel _energyViewModel;
        private readonly IEnergyDataService _energyDataService;
        private readonly ITweenersLocator _tweenersLocator;

        public MenuPopupState(
            IPopupService popupService,
            MenuViewModel menuViewModel,
            IRestartCommand restartCommand,
            IBackCommand backCommand,
            IContinueCommand continueCommand,
            IDisableButtonsCommand disableButtonsCommand,
            EnergyViewModel energyViewModel,
            IEnergyDataService energyDataService,
            ITweenersLocator tweenersLocator)
        {
            _popupService = popupService;
            _menuViewModel = menuViewModel;
            _restartCommand = restartCommand;
            _backCommand = backCommand;
            _continueCommand = continueCommand;
            _disableButtonsCommand = disableButtonsCommand;
            _energyViewModel = energyViewModel;
            _energyDataService = energyDataService;
            _tweenersLocator = tweenersLocator;
        }
        
        public async UniTask Enter(MenuViewModel menuViewModel)
        {
            Time.timeScale = 0f;
            MenuPopupView menuPopupView = _popupService.GetPopup<MenuPopupView>();
            
            menuPopupView.Initialize(
                _menuViewModel,
                _restartCommand,
                _backCommand,
                _continueCommand,
                _disableButtonsCommand,
                _energyViewModel,
                _energyDataService,
                _tweenersLocator
                );
            
            await menuPopupView.Show();
        }

        public async UniTask Exit()
        {
            Time.timeScale = 1f;
            await UniTask.CompletedTask;
        }
    }
}