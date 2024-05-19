using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace App.Scripts.External.Localisation.MonoBehaviours
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UILocale : MonoBehaviour
    {
        [SerializeField] private string _localeKey;
        [SerializeField] private TMP_Text _text;
        
        private ILocaleService _localeService;

        public TMP_Text Text => _text;
        public string Token => _localeKey;

        [Inject]
        private void Construct(ILocaleService localeService)
        {
            _localeService = localeService;
        }
        
        private void OnEnable()
        {
            SetText(_localeService.GetTextByToken(Token));

            _localeService.LocaleWasChanged += OnLocaleWasChanged;
        }

        private void OnDisable()
        {
            _localeService.LocaleWasChanged -= OnLocaleWasChanged;
        }

        private void OnLocaleWasChanged()
        {
            SetText(_localeService.GetTextByToken(Token));
        }

        public void SetToken(string token)
        {
            _localeKey = token;
            
            SetText(_localeService.GetTextByToken(Token));
        }

        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}