using System.Collections.Generic;

namespace General.Localisation
{
    public interface ILocaleProvider
    {
        Dictionary<string, string> LoadLocale(LocaleTypeId localeTypeId);
    }
}