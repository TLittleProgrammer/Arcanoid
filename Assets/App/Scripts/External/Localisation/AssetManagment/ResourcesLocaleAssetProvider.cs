using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.Scripts.External.Localisation.AssetManagment
{
    public sealed class ResourcesLocaleAssetProvider : ILocaleAssetProvider
    {
        private readonly string _basePath;
        private List<string> _allLocaleKeys;
        
        public ResourcesLocaleAssetProvider(string basePath)
        {
            _basePath = basePath.Replace("Assets/Resources/", String.Empty);
        }
        
        public TextAsset LoadTextByKey(string key)
        {
            string pathToAsset = $"{_basePath}/{key}";

            TextAsset result = Resources.Load<TextAsset>(pathToAsset);
            
            return result;
        }

        public List<string> GetAllLocaleKeys()
        {
            return _allLocaleKeys ?? LoadKeys();
        }

        private List<string> LoadKeys()
        {
            List<TextAsset> textAssets = Resources.LoadAll<TextAsset>(_basePath).ToList();
            List<string> result = new();

            foreach (TextAsset textAsset in textAssets)
            {
                result.Add(textAsset.name);
            }

            _allLocaleKeys = result;
            return result;
        }
    }
}