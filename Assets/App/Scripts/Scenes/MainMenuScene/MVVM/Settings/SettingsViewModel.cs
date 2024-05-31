using System.Collections.Generic;
using App.Scripts.General.Components;
using App.Scripts.Scenes.MainMenuScene.LocaleView;

namespace App.Scripts.Scenes.MainMenuScene.Popup
{
    public class SettingsViewModel : IViewModel
    {
        private readonly SettingsModel _settingsModel;

        public SettingsViewModel(SettingsModel settingsModel)
        {
            _settingsModel = settingsModel;
        }

        public void FillView(SettingsPopupView view)
        {
            List<LocaleItemView> localeItems = _settingsModel.GetLocaleItemViews();

            foreach (LocaleItemView itemView in localeItems)
            {
                itemView.transform.SetParent(view.LocaleItemViewParent, false);
            }
        }
    }
}