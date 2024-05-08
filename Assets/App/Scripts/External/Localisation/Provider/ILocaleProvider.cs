using System.Collections.Generic;

namespace App.Scripts.External.Localisation.Provider
{
    public interface ILocaleProvider
    {
        Dictionary<string, string> LoadLocale(LocaleTypeId localeTypeId);
    }
}