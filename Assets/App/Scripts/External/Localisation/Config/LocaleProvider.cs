using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.External.Localisation.Config
{
    [CreateAssetMenu(menuName = "Configs/Locale/Locale Provider", fileName = "LocaleProvider")]
    public class LocaleProvider : SerializedScriptableObject
    {
        public List<LocaleConfig> Configs;
        public Dictionary<string, TextAsset> LanguageAndTranslateMapping;
    }
}