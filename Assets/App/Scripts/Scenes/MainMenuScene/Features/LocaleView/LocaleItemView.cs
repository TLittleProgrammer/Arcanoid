using System;
using App.Scripts.External.Components;
using App.Scripts.External.Localisation.MonoBehaviours;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Features.LocaleView
{
    public class LocaleItemView : MonoBehaviour, ILocaleItemView, IClickable<string>
    {
        [SerializeField] private Image _languageImage;
        [SerializeField] private UILocale _locale;

        private string _targetLocaleKey;

        public event Action<string> Clicked;
        public GameObject GameObject => gameObject;

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(_targetLocaleKey);
        }

        public void SetModel(LocaleViewModel model)
        {
            _locale.SetToken(model.LocaleToken);
            _locale.SetText(model.LocaleTokenText);
            _languageImage.sprite = model.Sprite;
            _targetLocaleKey = model.LocaleKey;
        }

        public class Factory : PlaceholderFactory<LocaleItemView>
        {
            
        }
    }
}