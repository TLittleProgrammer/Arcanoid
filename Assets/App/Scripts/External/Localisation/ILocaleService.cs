using System;
using System.Collections.Generic;

namespace App.Scripts.External.Localisation
{
    public interface ILocaleService
    {
        string CurrentLanguageKey { get; }
        event Action LocaleWasChanged;
        void SetLocaleKey(string localeKey);
        string GetTextByToken(string token);
    }
}