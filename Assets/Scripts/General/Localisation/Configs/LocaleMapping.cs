using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace General.Localisation.Configs
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
    
    [Serializable]
    public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private List<TKey> keyData = new();
	
        [SerializeField, HideInInspector]
        private List<TValue> valueData = new();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();
            for (int i = 0; i < keyData.Count && i < valueData.Count; i++)
            {
                this[keyData[i]] = valueData[i];
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            keyData.Clear();
            valueData.Clear();

            foreach (var item in this)
            {
                keyData.Add(item.Key);
                valueData.Add(item.Value);
            }
        }
    }
}