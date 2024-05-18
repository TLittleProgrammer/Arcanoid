using System.IO;
using System.Net.Http;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.External.Localisation.Editor
{
    public class LocaleFromSheetToCsv
    {
        private const string GoogleSheetUrl = "https://docs.google.com/spreadsheets/d/16FRSUwuT03lxQGcOdGPKqrj3lb805kradf5N3YW_ue4";
        private const string GoogleSheetRequestTemplate = "{0}/export?format=csv&gid=0";
        private static readonly string LocalisationPath = Path.Combine("Assets", "Resources", "Configs", "Localisation", "Localisation.csv");

        [MenuItem("Tools/Locale/Open Locale")]
        private static void OpenLocalisation()
        {
            Application.OpenURL(GoogleSheetUrl);
        }

        [MenuItem("Tools/Locale/Download Locale")]
        private static async void DownloadLocalisation()
        {
            EditorUtility.DisplayProgressBar("Скачиваем локаль...", $"Попейте чайку", 0f);

            using HttpClient httpClient = new HttpClient();

            string url            = string.Format(GoogleSheetRequestTemplate, GoogleSheetUrl);
            var response          = await httpClient.GetAsync(url);
            string responseString = await response.Content.ReadAsStringAsync();
            
                    
            EditorUtility.DisplayProgressBar("Скачиваем локаль...", $"Попейте чайку", 95f);

            await File.WriteAllTextAsync(LocalisationPath, responseString);
            
            AssetDatabase.ImportAsset(LocalisationPath);
            EditorUtility.ClearProgressBar();
        }
    }
}