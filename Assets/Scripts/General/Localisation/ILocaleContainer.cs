using System.Collections.Generic;
using General.Initialization;

namespace General.Localisation
{
    public interface ILocaleContainer : IAsyncInitializable<Dictionary<string, string>>
    {
        string GetLocale(string localeKey);
    }
}