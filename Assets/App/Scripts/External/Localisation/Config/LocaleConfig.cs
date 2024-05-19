using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.External.Localisation.Config
{
    [Serializable]
    public class LocaleConfig
    {
        public string Key;
        [PreviewField(75)]
        public Sprite Sprite;
    }
}