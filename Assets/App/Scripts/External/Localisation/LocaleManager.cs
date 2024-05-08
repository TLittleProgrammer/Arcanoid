﻿using System;
using System.Collections.Generic;
using App.Scripts.External.Localisation.Provider;

namespace App.Scripts.External.Localisation
{
    public sealed class LocaleManager : ILocaleManager
    {
        public Action<LocaleTypeId> LanguageChanged { get; set; }
        public event Action LanguageUpdated;

        private readonly ILocaleContainer _localeContainer;
        private ILocaleProvider _localeProvider;

        public LocaleManager(ILocaleContainer localeContainer, ILocaleProvider localeProvider)
        {
            _localeContainer = localeContainer;
            _localeProvider = localeProvider;
            LanguageChanged += OnLanguageChanged;
        }

        private async void OnLanguageChanged(LocaleTypeId localeTypeId)
        {
            Dictionary<string, string> localeDictionary = _localeProvider.LoadLocale(localeTypeId);

            await _localeContainer.AsyncInitialize(localeDictionary);
            LanguageUpdated?.Invoke();
        }
    }
}