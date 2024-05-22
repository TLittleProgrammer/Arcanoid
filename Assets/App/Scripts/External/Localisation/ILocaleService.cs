using System;
using System.Collections.Generic;

namespace App.Scripts.External.Localisation
{
    public interface ILocaleService
    {
        event Action LocaleWasChanged;
        void SetLocale(string localeKey);
        void SetStorage(Dictionary<string, Dictionary<string, string>> storage);
        string GetTextByToken(string token);
    }
}