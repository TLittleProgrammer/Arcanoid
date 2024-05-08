using System.Collections.Generic;
using App.Scripts.External.Initialization;

namespace App.Scripts.External.Localisation
{
    public interface ILocaleContainer : IAsyncInitializable<Dictionary<string, string>>
    {
        string GetLocale(string localeKey);
    }
}