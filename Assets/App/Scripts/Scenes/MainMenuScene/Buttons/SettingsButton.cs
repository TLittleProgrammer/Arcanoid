using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.MainMenuScene.Popup;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.Bootstrap.Buttons
{
    public class SettingsButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private IPopupService _popupService;
        private SettingsModel _settingsModel;
        private SettingsViewModel _settingsViewModel;

        [Inject]
        private void Construct(IPopupService popupService, SettingsViewModel settingsViewModel)
        {
            _settingsViewModel = settingsViewModel;
            _popupService = popupService;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ShowSettingsPopup);
        }
        
        private void OnDisable()
        {
            _button.onClick.RemoveListener(ShowSettingsPopup);
        }
        
        private void ShowSettingsPopup()
        {
            SettingsPopupView settingsPopupView = _popupService.Show<SettingsPopupView>();
            _settingsViewModel.FillView(settingsPopupView);
        }
    }
}