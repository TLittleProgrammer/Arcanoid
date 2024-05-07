using System.Collections.Generic;

namespace General.Localisation.Provider
{
    public interface ILocaleProvider
    {
        Dictionary<string, string> LoadLocale(LocaleTypeId localeTypeId);
    }
}