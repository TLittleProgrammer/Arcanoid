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

        private Sequence _sequence;
        private IRestartService _restartService;

        public Button RestartButton => _restartButton;
        public Button BackButton => _backButton;
        public Button ContinueButton => _continueButton;
        public Transform Transform => transform;

        public override UniTask Show()
        {
            _sequence = DOTween.Sequence();
            _sequence
                .Append(transform.DOScale(Vector3.one, 1f).From(Vector3.zero))
                .Append(_restartButton.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce))
                .Append(_backButton.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce))
                .Append(_continueButton.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce));
            
            return UniTask.CompletedTask;
        }
    }
}