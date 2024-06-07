using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using App.Scripts.External.Localisation.Converters;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.External.Localisation.Editor
{
    public class LocaleFromSheetToCsv
    {
        private const string GoogleSheetUrl = "https://docs.google.com/spreadsheets/d/16FRSUwuT03lxQGcOdGPKqrj3lb805kradf5N3YW_ue4";
        private const string GoogleSheetRequestTemplate = "{0}/export?format=csv&gid=0";
        private static readonly string LocalisationPath = Path.Combine("Assets", "Resources", "Configs", "Localisation", "Localisation.csv");
        private static readonly string LocalisationPathToFolder = Path.Combine("Assets", "Resources", "Configs", "Localisation", "{0}.txt");

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

            CreateAndWriteLocaleToFiles(responseString);
            
            
            EditorUtility.ClearProgressBar();
        }

        private static async void CreateAndWriteLocaleToFiles(string allText)
        {
            string[,] csvGrid = new CsvConverter().ConvertFileToGrid(allText);
            Dictionary<string, Dictionary<string, string>> localeStorage = new();

            InitializeLocaleStorage(csvGrid, localeStorage);
            FillStorage(csvGrid, ref localeStorage);

            foreach ((string localeKey, Dictionary<string, string> localeMapping)  in localeStorage)
            {
                string writeTextToFile = string.Empty;

                foreach ((string key, string translate) in localeMapping)
                {
                    writeTextToFile += $"{key} {translate}\n";
                }
                
                await File.WriteAllTextAsync(string.Format(LocalisationPathToFolder, localeKey), writeTextToFile);
                
                AssetDatabase.ImportAsset(string.Format(LocalisationPathToFolder, localeKey));
            }
        }
        
        private static void InitializeLocaleStorage(string[,] csvGrid, Dictionary<string, Dictionary<string, string>> localeStorage)
        {
            for (int x = 1; x < csvGrid.GetLength(0); x++)
            {
                string languageKey = csvGrid[x, 0];

                if (!string.IsNullOrEmpty(languageKey))
                {
                    localeStorage.Add(languageKey, new Dictionary<string, string>());
                }
            }
        }

        private static void FillStorage(string[,] csvGrid, ref Dictionary<string, Dictionary<string, string>> localeStorage)
        {
            for (int i = 1; i < csvGrid.GetLength(0); i++)
            {
                string languageKey = csvGrid[i, 0];

                if (string.IsNullOrEmpty(languageKey))
                    continue;

                for (int j = 1; j < csvGrid.GetLength(1) - 1; j++)
                {
                    if (csvGrid[0, j] is not null)
                    {
                        localeStorage[languageKey].Add(csvGrid[0, j], csvGrid[i, j]);
                    }
                }
            }
        }
    }
}