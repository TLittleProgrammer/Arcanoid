using System.Threading.Tasks;
using App.Scripts.General.Animator;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Popups.Animators
{
    public class ShowWinAnimator : MonoAnimator<ITweenersLocator>
    {
        [SerializeField] private Transform _energyView;
        [SerializeField] private Transform _continueButton;
        [SerializeField] private Transform _packView;
        [SerializeField] private Transform _winPopup;
        [SerializeField] private Transform _targetPositionForTextFalling;
        [SerializeField] private RectTransform _textFalling;
        
        public override async UniTask Animate(ITweenersLocator tweenersLocator)
        {
            SetDefaultScale();
            await ShowMainInformation();
            AnimateTeextFalling();
            await AnimateContinueButton();
            SetBouncingToContinueButton(tweenersLocator);
        }

        private void SetBouncingToContinueButton(ITweenersLocator tweenersLocator)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(
                    _continueButton
                        .transform
                        .DOScale(Vector3.one,
                            0.25f)
                        .SetDelay(2f)
                        .SetLoops(2, LoopType.Yoyo))
                .SetEase(Ease.OutCubic)
                .SetLoops(-1, LoopType.Restart)
                .ToUniTask().Forget();
            
            tweenersLocator.AddSequence(sequence);
        }

        private void AnimateTeextFalling()
        {
            _textFalling.DOMoveY(_targetPositionForTextFalling.position.y, 0.5f).SetEase(Ease.OutBounce).ToUniTask().Forget();
        }

        private async Task AnimateContinueButton()
        {
            await _continueButton.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBounce).ToUniTask();
        }

        private async Task ShowMainInformation()
        {
            await _winPopup.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBounce).ToUniTask();
            await _energyView.transform.DOScale(Vector3.one, 0.75f).From(Vector3.zero).SetEase(Ease.OutBounce).ToUniTask();
            await _packView.transform.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutBounce).ToUniTask();
        }

        private void SetDefaultScale()
        {
            _energyView.transform.localScale = Vector3.zero;
            _continueButton.transform.localScale = Vector3.zero;
            _packView.transform.localScale = Vector3.zero;
        }

        public override UniTask UndoAnimate()
        {
            return UniTask.CompletedTask;
        }
    }
}