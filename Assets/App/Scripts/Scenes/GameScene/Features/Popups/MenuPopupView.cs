using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Restart;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Popups
{
    public class MenuPopupView : PopupView, IMenuPopupView
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _skipLevelButton;

        private Sequence _sequence;
        private IRestartService _restartService;

        public Button RestartButton => _restartButton;
        public Button BackButton => _backButton;
        public Button ContinueButton => _continueButton;
        public Button SkipLevelButton => _skipLevelButton;
        public Transform Transform => transform;

        public override UniTask Show()
        {
            return UniTask.CompletedTask;
        }
    }
}