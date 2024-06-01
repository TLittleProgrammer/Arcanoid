using App.Scripts.General.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Popups
{
    public class LoosePopupView : PopupView, ILoosePopupView
    {
        [SerializeField] private Button _restartButton;
        public Button RestartButton => _restartButton;
    }
}