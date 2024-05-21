using System.Collections.Generic;
using App.Scripts.External.Localisation;
using App.Scripts.External.Localisation.Converters;
using App.Scripts.General.ProjectInitialization.Settings;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.ProjectInitialization.Installers
{
    public class ProjectInitializer : IInitializable
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly ILocaleService _localeService;
        private readonly IConverter _converter;
        private readonly TextAsset _localisation;

        public ProjectInitializer(
            ApplicationSettings applicationSettings,
            ILocaleService localeService,
            IConverter converter,
            TextAsset localisation)
        {
            _applicationSettings = applicationSettings;
            _localeService = localeService;
            _converter = converter;
            _localisation = localisation;
        }
        
        public void Initialize()
        {
            Application.targetFrameRate = _applicationSettings.TargetFPS;
            QualitySettings.vSyncCount = _applicationSettings.VSyncCounter;
            
            Debug.Log("CYKA");

            InitializeLocale();
        }

        private void InitializeLocale()
        {
            string[,] csvGrid = _converter.ConvertFileToGrid(_localisation.text);

            Dictionary<string, Dictionary<string, string>> localeStorage = new();

            InitializeLocaleStorage(csvGrid, localeStorage);
            FillStorage(csvGrid, ref localeStorage);
            
            _localeService.SetStorage(localeStorage);
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

        private void FillStorage(string[,] csvGrid, ref Dictionary<string, Dictionary<string, string>> localeStorage)
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