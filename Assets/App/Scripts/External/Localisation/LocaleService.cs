using System;
using System.Collections.Generic;
using App.Scripts.External.Initialization;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.External.Localisation
{
    public interface ILocaleService
    {
        event Action LocaleWasChanged;
        void SetLocale(string localeKey);
        string GetTextByToken(string token);
    }
    
    public class LocaleService : ILocaleService, IAsyncInitializable<string>
    {
        private Dictionary<string, Dictionary<string, string>> _localeStorage;
        private string _currentLanguageKey = LocaleConstants.DefaultLocaleKey;

        public event Action LocaleWasChanged;

        public LocaleService(string localisationContent)
        {
            AsyncInitialize(localisationContent).Forget();
        }

        public async UniTask AsyncInitialize(string localisationContent)
        {
            string[,] csvGrid = CsvFileService.SplitCsvGrid(localisationContent);

            _localeStorage = new();

            for (int x = 1; x < csvGrid.GetLength(0); x++)
            {
                string languageKey = csvGrid[x, 0];

                if (!string.IsNullOrEmpty(languageKey))
                {
                    _localeStorage.Add(languageKey, new Dictionary<string, string>());
                }
            }

            InitStorage(csvGrid);

            await UniTask.CompletedTask;
        }

        public void SetLocale(string localeKey)
        {
            string targetKey = _localeStorage.ContainsKey(localeKey) ? localeKey : LocaleConstants.DefaultLocaleKey;

            if (!targetKey.Equals(_currentLanguageKey, StringComparison.CurrentCultureIgnoreCase))
            {
                _currentLanguageKey = targetKey;
                LocaleWasChanged?.Invoke();
            }
        }

        public string GetTextByToken(string token)
        {
            if (_localeStorage[_currentLanguageKey].ContainsKey(token))
            {
                return _localeStorage[_currentLanguageKey][token];
            }

            return LocaleConstants.TokenNotFoundText;
        }

        private void InitStorage(string[,] csvGrid)
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
                        _localeStorage[languageKey].Add(csvGrid[0, j], csvGrid[i, j]);
                    }
                }
            }
        }
    }
}