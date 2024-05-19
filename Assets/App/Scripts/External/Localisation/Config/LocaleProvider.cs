using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.External.Localisation.Config
{
    [CreateAssetMenu(menuName = "Configs/Locale/Locale Provider", fileName = "LocaleProvider")]
    public class LocaleProvider : ScriptableObject
    {
        public List<LocaleConfig> Configs;
    }
}