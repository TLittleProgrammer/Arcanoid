using App.Scripts.General.Popup;
using App.Scripts.Scenes.MainMenuScene.LocaleView;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Popup
{
    public class SettingsPopupView : PopupView
    {
        [SerializeField] private Transform _localeItemViewParent;
        
        private LocaleItemView.Factory _localeItemViewFactory;

        [Inject]
        private void Construct(LocaleItemView.Factory localeItemViewFactory)
        {
            _localeItemViewFactory = localeItemViewFactory;
        }

        private void OnEnable()
        {
            var items = _localeItemViewFactory.Create();

            foreach (LocaleItemView localeItemView in items)
            {
                localeItemView.transform.SetParent(_localeItemViewParent, false);
            }
        }
    }
}