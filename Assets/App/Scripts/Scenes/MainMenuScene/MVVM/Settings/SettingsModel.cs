using System.Collections.Generic;
using App.Scripts.External.Localisation;
using App.Scripts.External.Localisation.Config;
using App.Scripts.General.Components;
using App.Scripts.Scenes.MainMenuScene.Features.LocaleView;

namespace App.Scripts.Scenes.MainMenuScene.MVVM.Settings
{
    public class SettingsModel : IModel
    {
        private readonly LocaleItemView.Factory _localeItemViewFactory;
        private readonly LocaleProvider _localeProvider;
        private readonly ILocaleService _localeService;

        private const string LocaleTokenPostfix = "_itemView";

        private SettingsModel(
            LocaleItemView.Factory localeItemViewFactory,
            LocaleProvider localeProvider,
            ILocaleService localeService)
        {
            _localeItemViewFactory = localeItemViewFactory;
            _localeProvider = localeProvider;
            _localeService = localeService;
        }
        
        public List<LocaleItemView> GetLocaleItemViews()
        {
            List<LocaleItemView> localeItemViews = new();

            foreach (LocaleConfig localeConfig in _localeProvider.Configs)
            {
                LocaleItemView item = _localeItemViewFactory.Create();

                UpdateView(localeConfig, item);

                localeItemViews.Add(item);
            }
            
            return localeItemViews;
        }

        private void UpdateView(LocaleConfig localeConfig, LocaleItemView item)
        {
            LocaleViewModel model = new();
            model.Sprite = localeConfig.Sprite;
            model.LocaleKey = localeConfig.Key;
            model.LocaleToken = $"{localeConfig.Key}{LocaleTokenPostfix}";
            model.LocaleTokenText = _localeService.GetTextByToken(model.LocaleToken);

            item.SetModel(model);
        }
    }
}