using System.Collections.Generic;
using App.Scripts.General.Animator;
using App.Scripts.General.Command;
using App.Scripts.General.Components;
using App.Scripts.General.Energy;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.Popup;
using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Loose;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Popups
{
    public class LoosePopupView : PopupView, ILoosePopupView
    {
        [SerializeField] private GameObject _hideClicksPanel;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private EnergyView _energyView;
        [SerializeField] private TMP_Text _priceToRestart;
        [SerializeField] private TMP_Text _priceToContinue;
        [SerializeField] private MonoAnimator[] _showAnimators;
        
        private LooseViewModel _viewModel;
        private IRestartCommand _restartCommand;
        private IBackCommand _backCommand;
        private IDisableButtonsCommand _disableButtonsCommand;
        private IBuyHealthCommand _buyHealthCommand;
        private IEnergyDataService _energyDataService;

        public Button RestartButton => _restartButton;
        
        public void SetUp(
            LooseViewModel viewModel,
            EnergyViewModel energyViewModel,
            IEnergyDataService energyDataService,
            IRestartCommand restartCommand,
            IBackCommand backCommand,
            IDisableButtonsCommand disableButtonsCommand,
            IBuyHealthCommand buyHealthCommand)
        {
            _energyDataService = energyDataService;
            _viewModel = viewModel;
            _restartCommand = restartCommand;
            _backCommand = backCommand;
            _disableButtonsCommand = disableButtonsCommand;
            _buyHealthCommand = buyHealthCommand;
            
            _restartButton.onClick.AddListener(Restart);
            _backButton.onClick.AddListener(Back);
            _continueButton.onClick.AddListener(Continue);
            
            _energyView.Initialize(energyViewModel);
            _priceToContinue.text = viewModel.GetPriceToContinue().ToString();
            _priceToRestart.text = viewModel.GetPriceToRestart().ToString();
            
            _energyDataService.ValueChanged += OnEnergyValueChanged;
            RedrawContinueButton(_energyDataService.CurrentValue);
        }

        private void OnDisable()
        {
            _energyDataService.ValueChanged -= OnEnergyValueChanged;
        }

        public override async UniTask Show()
        {
            foreach (MonoAnimator monoAnimator in _showAnimators)
            {
                await monoAnimator.Animate();
            }
            
            _hideClicksPanel.SetActive(false);
        }

        public override async UniTask Close()
        {
            for (int i = _showAnimators.Length - 1; i >= 0; i--)
            {
                await _showAnimators[i].UndoAnimate();
            }
        }

        private void Back()
        {
            ExecuteCommand(_backCommand);
        }

        private void Continue()
        {
            ExecuteCommand(_buyHealthCommand);
        }

        private void Restart()
        {
            ExecuteCommand(_restartCommand);
        }

        private void ExecuteCommand(ICommand command)
        {
            List<Button> buttons = new()
            {
                _continueButton,
                _restartButton,
                _backButton,
            };
            
            _disableButtonsCommand.Execute(buttons);
            
            command.Execute();
        }

        private void OnEnergyValueChanged(int energyValue)
        {
            RedrawRestartButton(energyValue);
            RedrawContinueButton(energyValue);
        }

        private void RedrawRestartButton(int energyValue)
        {
            _restartButton.interactable = _viewModel.CanRestart(energyValue);
        }

        private void RedrawContinueButton(int energyValue)
        {
            _continueButton.interactable = _viewModel.CanContinue(energyValue);
        }
    }
}