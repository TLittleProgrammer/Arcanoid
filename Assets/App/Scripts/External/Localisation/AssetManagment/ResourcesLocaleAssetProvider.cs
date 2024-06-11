using System;
using System.IO;
using Codice.Client.Commands;
using UnityEngine;

namespace App.Scripts.External.Localisation.AssetManagment
{
    public sealed class ResourcesLocaleAssetProvider : ILocaleAssetProvider
    {
        private readonly string _basePath;
        
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
    }
}