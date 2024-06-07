using System.Collections.Generic;

namespace App.Scripts.External.Localisation
{
    public record LocaleData
    {
        public string Key;
        public Dictionary<string, string> Translates;
    }
}