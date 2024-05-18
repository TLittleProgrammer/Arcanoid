using App.Scripts.General.Popup;
using App.Scripts.Scenes.MainMenuScene.Popup;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.Bootstrap.Buttons
{
    public class ContinueButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
        private IPopupService _popupService;

        [Inject]
        private void Construct(IPopupService popupService)
        {
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
            _popupService.Close<SettingsPopupView>();
        }
    }
}