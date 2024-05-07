using System.Collections.Generic;

namespace External.Localisation.Provider
{
    public interface ILocaleProvider
    {
        Dictionary<string, string> LoadLocale(LocaleTypeId localeTypeId);
    }
}