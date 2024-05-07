using System;

namespace General.Localisation
{
    public interface ILocaleManager
    {
        Action<LocaleTypeId> LanguageChanged { get; set; }
        event Action LanguageUpdated;
    }
}