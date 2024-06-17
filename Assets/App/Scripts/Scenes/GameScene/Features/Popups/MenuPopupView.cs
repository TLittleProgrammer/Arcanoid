﻿using System;
using System.Collections.Generic;
using App.Scripts.General.Animator;
using App.Scripts.General.Command;
using App.Scripts.General.Components;
using App.Scripts.General.Energy;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.Popup;
using App.Scripts.General.UserData.Energy;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Restart;
using App.Scripts.Scenes.GameScene.MVVM.Popups.Menu;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Popups
{
    public class MenuPopupView : PopupView, IMenuPopupView
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private EnergyView _energyView;
        [SerializeField] private TMP_Text _priceToRestart;
        [SerializeField] private MonoAnimator _showAnimator;
        [SerializeField] private MonoAnimator<ITweenersLocator> _closeAnimator;

        private Sequence _sequence;
        private IRestartService _restartService;
        private IRestartCommand _restartCommand;
        private IBackCommand _backCommand;
        private IContinueCommand _continueCommand;
        private IDisableButtonsCommand _disableButtonsCommand;
        private MenuViewModel _menuViewModel;
        private IEnergyDataService _energyDataService;
        private ITweenersLocator _tweenersLocator;

        public Button RestartButton => _restartButton;
        public Button BackButton => _backButton;
        public Button ContinueButton => _continueButton;
        public Transform Transform => transform;

        public void Initialize(
            MenuViewModel menuViewModel,
            IRestartCommand restartCommand,
            IBackCommand backCommand,
            IContinueCommand continueCommand,
            IDisableButtonsCommand disableButtonsCommand,
            EnergyViewModel energyViewModel,
            IEnergyDataService energyDataService,
            ITweenersLocator tweenersLocator)
        {
            _tweenersLocator = tweenersLocator;
            _energyDataService = energyDataService;
            _menuViewModel = menuViewModel;
            _disableButtonsCommand = disableButtonsCommand;
            _continueCommand = continueCommand;
            _backCommand = backCommand;
            _restartCommand = restartCommand;

            _priceToRestart.text = _menuViewModel.GetPriceToRestart().ToString();
            
            _energyView.Initialize(energyViewModel);
            SubscribeOnCommands();
            SubsribeOnEnergyDataService();
        }

        private void OnDestroy()
        {
            _energyDataService.ValueChanged -= OnEnergyValueChanged;
        }

        public override UniTask Show()
        {
            return _showAnimator.Animate();
        }

        public override UniTask Close()
        {
            return _closeAnimator.Animate(_tweenersLocator);
        }

        private void SubsribeOnEnergyDataService()
        {
            _energyDataService.ValueChanged += OnEnergyValueChanged;
            OnEnergyValueChanged(_energyDataService.CurrentValue);
        }

        private void OnEnergyValueChanged(int energyValue)
        {
            RedrawRestartButton(energyValue);
        }

        private void RedrawRestartButton(int energyValue)
        {
            _restartButton.interactable = energyValue >= _menuViewModel.GetPriceToRestart();
        }

        private void SubscribeOnCommands()
        {
            _continueButton.onClick.AddListener(Continue);
            _restartButton.onClick.AddListener(Restart);
            _backButton.onClick.AddListener(Back);
        }

        private void Restart()
        {
            ExecuteCommand(_restartCommand);
        }

        private void Continue()
        {
            ExecuteCommand(_continueCommand);
        }

        private void Back()
        {
            ExecuteCommand(_backCommand);
        }

        private void ExecuteCommand(ICommand command)
        {
            DisableButtons();
            
            command.Execute();
        }
        
        private void DisableButtons()
        {
            List<Button> buttons = new List<Button>
            {
                _backButton,
                _continueButton,
                _restartButton,
            };
            
            _disableButtonsCommand.Execute(buttons);
        }
    }
}