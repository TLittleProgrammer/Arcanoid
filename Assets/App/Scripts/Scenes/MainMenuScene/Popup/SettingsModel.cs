using System.Collections.Generic;
using App.Scripts.General.Components;
using App.Scripts.Scenes.MainMenuScene.LocaleView;

namespace App.Scripts.Scenes.MainMenuScene.Popup
{
    public class SettingsModel : IModel
    {
        private readonly LocaleItemView.Factory _localeItemViewFactory;

        private SettingsModel(LocaleItemView.Factory localeItemViewFactory)
        {
            _localeItemViewFactory = localeItemViewFactory;
        }
        
        public List<LocaleItemView> GetLocaleItemViews()
        {
            return _localeItemViewFactory.Create();
        }
    }
}