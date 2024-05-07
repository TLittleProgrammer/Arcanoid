using System.Collections.Generic;
using External.Initialization;

namespace External.Localisation
{
    public interface ILocaleContainer : IAsyncInitializable<Dictionary<string, string>>
    {
        string GetLocale(string localeKey);
    }
}