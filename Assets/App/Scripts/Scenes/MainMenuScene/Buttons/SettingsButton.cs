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
        private RootUIViewProvider _rootUIViewProvider;

        [Inject]
        private void Construct(IPopupService popupService, RootUIViewProvider rootUIViewProvider)
        {
            _rootUIViewProvider = rootUIViewProvider;
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
            _popupService.Show<SettingsPopupView>(_rootUIViewProvider.PopupUpViewProvider);
        }
    }
}