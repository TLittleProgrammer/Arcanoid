using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.General.Components;
using App.Scripts.Scenes.GameScene.Command;
using App.Scripts.Scenes.MainMenuScene.Command;
using App.Scripts.Scenes.MainMenuScene.LocaleView;
using App.Scripts.Scenes.MainMenuScene.MVVM.Settings;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.MainMenuScene.Popup
{
    public class SettingsViewModel : IViewModel
    {
        private readonly SettingsModel _settingsModel;
        private readonly IChangeLocaleCommand _changeLocaleCommand;
        private readonly IContinueCommand _continueCommand;
        private readonly IDisableButtonsCommand _disableButtonsCommand;
        private SettingsPopupView _view;

        public SettingsViewModel(
            SettingsModel settingsModel,
            IChangeLocaleCommand changeLocaleCommand,
            IContinueCommand continueCommand,
            IDisableButtonsCommand disableButtonsCommand)
        {
            _settingsModel = settingsModel;
            _changeLocaleCommand = changeLocaleCommand;
            _continueCommand = continueCommand;
            _disableButtonsCommand = disableButtonsCommand;
        }

        public void FillView(SettingsPopupView view)
        {
            _view = view;
            SubscribeOnButtonsClick(view);
            CreateLocaleItems(view);
            AnimateShowing(view);
        }

        private void CreateLocaleItems(SettingsPopupView view)
        {
            List<LocaleItemView> localeItems = _settingsModel.GetLocaleItemViews();

            foreach (LocaleItemView itemView in localeItems)
            {
                ItemSubscribeOnClick(itemView);
                itemView.transform.SetParent(view.LocaleItemViewParent, false);
            }
        }

        private void SubscribeOnButtonsClick(SettingsPopupView settingsPopupView)
        {
            settingsPopupView.ContinueButton.onClick.AddListener(Continue);
        }

        private void Continue()
        {
            DisableButtons();

            _continueCommand.Execute(_view.transform);
        }

        private void DisableButtons()
        {
            List<Button> buttons = new()
            {
                _view.ContinueButton
            };

            _disableButtonsCommand.Execute(buttons);
        }

        private void AnimateShowing(SettingsPopupView settingsPopupView)
        {
            settingsPopupView.transform.DOScale(Vector3.one, 0.75f).From(Vector3.zero);
        }

        private void ItemSubscribeOnClick(IClickable<string> clickable)
        {
            clickable.Clicked += _changeLocaleCommand.Execute;
        }
    }
}