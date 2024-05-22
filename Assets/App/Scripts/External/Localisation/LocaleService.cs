using System;
using System.Collections.Generic;

namespace App.Scripts.External.Localisation
{
    public class LocaleService : ILocaleService
    {
        private Dictionary<string, Dictionary<string, string>> _localeStorage = new();
        private string _currentLanguageKey = LocaleConstants.DefaultLocaleKey;

        public event Action LocaleWasChanged;

        public void SetStorage(Dictionary<string, Dictionary<string, string>> storage)
        {
            _localeStorage = storage;
            
            LocaleWasChanged?.Invoke();
        }

        public void SetLocale(string localeKey)
        {
            string targetKey = _localeStorage.ContainsKey(localeKey) ? localeKey : LocaleConstants.DefaultLocaleKey;

            if (!targetKey.Equals(_currentLanguageKey, StringComparison.CurrentCultureIgnoreCase))
            {
                _currentLanguageKey = targetKey;
                LocaleWasChanged?.Invoke();
            }
        }

        public string GetTextByToken(string token)
        {
            if (!_localeStorage.ContainsKey(_currentLanguageKey))
            {
                return string.Format(LocaleConstants.LanguageNotFound, _currentLanguageKey);
            }
            
            if (_localeStorage[_currentLanguageKey].ContainsKey(token))
            {
                return _localeStorage[_currentLanguageKey][token];
            }

            return LocaleConstants.TokenNotFoundText;
        }
    }
}