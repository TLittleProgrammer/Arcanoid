using System.Collections.Generic;
using App.Scripts.External.Localisation;
using App.Scripts.External.Localisation.AssetManagment;
using App.Scripts.External.Localisation.Config;
using App.Scripts.General.Components;
using App.Scripts.General.Providers;
using App.Scripts.Scenes.MainMenuScene.Features.LocaleView;
using UnityEngine;

namespace App.Scripts.Scenes.MainMenuScene.MVVM.Settings
{
    public class SettingsModel : IModel
    {
        private readonly LocaleItemView.Factory _localeItemViewFactory;
        private readonly ILocaleService _localeService;
        private readonly ILocaleAssetProvider _localeAssetProvider;
        private readonly SpriteProvider _spriteProvider;

        private const string LocaleTokenPostfix = "_itemView";
        private const string LocaleFlagPostfix = "_flag";

        private SettingsModel(
            LocaleItemView.Factory localeItemViewFactory,
            ILocaleService localeService,
            ILocaleAssetProvider localeAssetProvider,
            SpriteProvider spriteProvider)
        {
            _localeItemViewFactory = localeItemViewFactory;
            _localeService = localeService;
            _localeAssetProvider = localeAssetProvider;
            _spriteProvider = spriteProvider;
        }
        
        public List<LocaleItemView> GetLocaleItemViews()
        {
            List<LocaleItemView> localeItemViews = new();
            List<string> localeKeys = _localeAssetProvider.GetAllLocaleKeys();

            foreach (string key in localeKeys)
            {
                LocaleItemView item = _localeItemViewFactory.Create();

                UpdateView(key, item);

                localeItemViews.Add(item);
            }
            
            return localeItemViews;
        }

        private void UpdateView(string key, LocaleItemView item)
        {
            string token = GetLocaleToken(key);
            Sprite sprite = _spriteProvider.Sprites[GetFlagId(key)];
            
            LocaleViewModel model = new();
            model.Sprite = sprite;
            model.LocaleKey = key;
            model.LocaleToken = token;
            model.LocaleTokenText = _localeService.GetTextByToken(model.LocaleToken);

            item.SetModel(model);
        }

        private string GetFlagId(string key)
        { 
            return $"{key}{LocaleFlagPostfix}";
        }

        private static string GetLocaleToken(string key)
        {
            return $"{key}{LocaleTokenPostfix}";
        }
    }
}