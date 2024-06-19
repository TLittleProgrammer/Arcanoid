using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.External.Localisation.AssetManagment
{
    public interface ILocaleAssetProvider
    {
        TextAsset LoadTextByKey(string key);
        List<string> GetAllLocaleKeys();
    }
}