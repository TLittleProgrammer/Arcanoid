using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.General.Components;
using App.Scripts.Scenes.MainMenuScene.Command;
using App.Scripts.Scenes.MainMenuScene.LocaleView;
using App.Scripts.Scenes.MainMenuScene.MVVM.Settings;

namespace App.Scripts.Scenes.MainMenuScene.Popup
{
    public class SettingsViewModel : IViewModel
    {
        private readonly SettingsModel _settingsModel;
        private readonly IChangeLocaleCommand _changeLocaleCommand;

        public SettingsViewModel(SettingsModel settingsModel, IChangeLocaleCommand changeLocaleCommand)
        {
            _settingsModel = settingsModel;
            _changeLocaleCommand = changeLocaleCommand;
        }

        public void FillView(SettingsPopupView view)
        {
            List<LocaleItemView> localeItems = _settingsModel.GetLocaleItemViews();

            foreach (LocaleItemView itemView in localeItems)
            {
                SubscribeOnClick(itemView);
                itemView.transform.SetParent(view.LocaleItemViewParent, false);
            }
        }
        
        private void SubscribeOnClick(IClickable<string> clickable)
        {
            clickable.Clicked += _changeLocaleCommand.Execute;
        }
    }
}