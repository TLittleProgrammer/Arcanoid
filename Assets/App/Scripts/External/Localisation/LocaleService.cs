using System;
using System.Collections.Generic;

namespace App.Scripts.External.Localisation
{
    public class LocaleService : ILocaleService
    {
        private readonly ILocaleMappingFromTextAsset _localeMappingFromTextAsset;
        
        private Dictionary<string, string> _localeStorage = new();
        private string _currentLanguageKey;

        public string CurrentLanguageKey => _currentLanguageKey;
        public event Action LocaleWasChanged;

        public LocaleService(ILocaleMappingFromTextAsset localeMappingFromTextAsset)
        {
            _localeMappingFromTextAsset = localeMappingFromTextAsset;
        }

        public void SetLocaleKey(string localeKey)
        {
            LocaleData localeData = _localeMappingFromTextAsset.GetLocaleMapping(localeKey);
            
            if (!localeData.Key.Equals(_currentLanguageKey, StringComparison.CurrentCultureIgnoreCase))
            {
                _localeStorage = localeData.Translates;
                _currentLanguageKey = localeData.Key;
                LocaleWasChanged?.Invoke();
            }
        }

        public string GetTextByToken(string token)
        {
            if (_localeStorage.ContainsKey(token))
            {
                return _localeStorage[token];
            }

            return LocaleConstants.TokenNotFoundText;
        }
    }
}