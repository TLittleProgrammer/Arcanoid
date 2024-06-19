using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.Localisation.AssetManagment;
using App.Scripts.External.Localisation.Config;
using UnityEngine;

namespace App.Scripts.External.Localisation
{
    public sealed class LocaleMappingFromTextAsset : ILocaleMappingFromTextAsset
    {
        private readonly ILocaleAssetProvider _localeAssetProvider;
        private List<string> _availableKeys;

        public LocaleMappingFromTextAsset(ILocaleAssetProvider localeAssetProvider)
        {
            _localeAssetProvider = localeAssetProvider;
        }
        
        public LocaleData GetLocaleMapping(string localeKey)
        {
            TextAsset localeMapping = _localeAssetProvider.LoadTextByKey(localeKey);

            if (localeMapping is null)
            {
                return null;
            }
            
            Dictionary<string, string> translates = GetTranslatesFromFile(localeMapping);

            return new LocaleData
            {
                Key = localeKey,
                Translates = translates
            };
        }

        private Dictionary<string, string> GetTranslatesFromFile(TextAsset localeMapping)
        {
            Dictionary<string, string> translates = new();

            string[] lines = localeMapping.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            
            foreach (string line in lines)
            {
                if(line.Equals(string.Empty))
                    continue;
                
                int firstSpaceIndex = line.IndexOf(' ');
                
                string key = line.Substring(0, firstSpaceIndex);
                string translate = line.Substring(firstSpaceIndex + 1);

                translates.Add(key, translate);
            }

            return translates;
        }
    }
}