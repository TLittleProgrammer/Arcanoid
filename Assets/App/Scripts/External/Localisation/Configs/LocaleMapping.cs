using System;
using System.Collections.Generic;
using External.Odin;
using Sirenix.OdinInspector;
using UnityEngine;

namespace External.Localisation.Configs
{
    [CreateAssetMenu(menuName = "Configs/LocaleMapping", fileName = "LocaleMapping")]
    public class LocaleMapping : SerializedScriptableObject
    {
        public List<LocaleKeyMapping> Mapping;
    }

    [Serializable]
    public sealed class LocaleKeyMapping
    {
        public string LocaleKey;
        public TranslationsMappingDictionary Translations;
    }
    
    [Serializable]
    public class TranslationsMappingDictionary : UnitySerializedDictionary<LocaleTypeId, string> { }
}