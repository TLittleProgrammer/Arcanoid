using System;

namespace External.Localisation
{
    public interface ILocaleManager
    {
        Action<LocaleTypeId> LanguageChanged { get; set; }
        event Action LanguageUpdated;
    }
}