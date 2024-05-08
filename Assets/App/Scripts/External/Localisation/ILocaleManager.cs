using System;

namespace App.Scripts.External.Localisation
{
    public interface ILocaleManager
    {
        Action<LocaleTypeId> LanguageChanged { get; set; }
        event Action LanguageUpdated;
    }
}