using System;
using System.Collections.Generic;
using System.Linq;
using External.Localisation.Configs;
using UnityEngine;

namespace External.Localisation.Provider
{
    public sealed class ResourcesLocaleProvider : ILocaleProvider
    {
        private const string PathToLocale = "Configs/Localisation/LocaleMapping";
        
        public Dictionary<string, string> LoadLocale(LocaleTypeId localeTypeId)
        {
            LocaleMapping localeMapping = Resources.Load<LocaleMapping>(PathToLocale);
            
            return localeMapping
                .Mapping
                .ToDictionary(
                    x => x.LocaleKey,
                    x => ChooseTranslation(localeTypeId, x));
        }

        private string ChooseTranslation(LocaleTypeId localeTypeId, LocaleKeyMapping keyMapping)
        {
            return keyMapping
                .Translations
                .TryGetValue(localeTypeId, out string locale) ? locale : String.Empty;
        }
    }
}