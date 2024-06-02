﻿using App.Scripts.General.Components;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.MainMenuScene.MVVM.Settings;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Features.Buttons
{
    public class ButtonsHandler : IInitializable
    {
        private readonly IButtonable _settingsButton;
        private readonly IPopupService _popupService;
        private readonly SettingsViewModel _settingsViewModel;

        public ButtonsHandler(
            IButtonable settingsButton,
            IPopupService popupService,
            SettingsViewModel settingsViewModel)
        {
            _settingsButton = settingsButton;
            _popupService = popupService;
            _settingsViewModel = settingsViewModel;
        }

        public void Initialize()
        {
            _settingsButton.Button.onClick.AddListener(ShowSettings);
        }

        private void ShowSettings()
        {
            SettingsPopupView settingsView = ShowPopup();
            _settingsViewModel.FillView(settingsView);
        }

        private SettingsPopupView ShowPopup()
        {
            return _popupService.Show<SettingsPopupView>();
        }
    }
}