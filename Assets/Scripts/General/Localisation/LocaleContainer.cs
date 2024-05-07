using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace General.Localisation
{
    public sealed class LocaleContainer : ILocaleContainer
    {
        private Dictionary<string, string> _localeContainer;

        public async UniTask AsyncInitialize(Dictionary<string, string> param)
        {
            _localeContainer?.Clear();
            _localeContainer = param;

            await UniTask.CompletedTask;
        }

        public string GetLocale(string localeKey)
        {
            return _localeContainer.TryGetValue(localeKey, out string value) ? value : String.Empty;
        }
    }
}