using System.Collections.Generic;
using App.Scripts.External.Localisation;
using App.Scripts.External.Localisation.MonoBehaviours;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.LocaleView
{
    public class LocaleItemView : MonoBehaviour, ILocaleItemView, IPointerClickHandler
    {
        [SerializeField] private Image _languageImage;
        [SerializeField] private UILocale _locale;

        private ILocaleService _localeService;
        private string _targetLocaleKey;

        [Inject]
        private void Construct(ILocaleService localeService)
        {
            _localeService = localeService;
        }

        public GameObject GameObject => gameObject;

        public void OnPointerClick(PointerEventData eventData)
        {
            _localeService.SetLocale(_targetLocaleKey);
        }

        public void SetModel(LocaleViewModel model)
        {
            _languageImage.sprite = model.Sprite;
            _locale.SetToken(model.LocaleToken);
            _targetLocaleKey = model.LocaleKey;
            
            UpdateLocale();
        }

        private void UpdateLocale()
        {
            string viewText = _localeService.GetTextByToken(_locale.Token);
            _locale.SetText(viewText);
        }

        public class Factory : PlaceholderFactory<List<LocaleItemView>>
        {
            
        }
    }
}