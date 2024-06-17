using App.Scripts.General.Animator;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.MVVM.Popups.Menu
{
    public class ShowMenuAnimator : MonoAnimator
    {
        [SerializeField] private Transform _energy;
        [SerializeField] private Transform _continueButton;
        [SerializeField] private Transform _backButton;
        [SerializeField] private Transform _restartButton;
        [SerializeField] private Transform _menuView;
        [SerializeField] private TMP_Text _menuLabel;

        public override UniTask Animate()
        {
            Sequence sequence = DOTween.Sequence();
            sequence
                .SetUpdate(true)
                .Append(_menuView.DOScale(Vector3.one, 1f).From(Vector3.zero))
                .Append(_energy.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce))
                .Append(_restartButton.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce))
                .Append(_continueButton.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce))
                .Append(_backButton.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBounce));

            DOVirtual
                .Float(0f, 1f, 1f, ShowObjectsText)
                .SetDelay(1.25f)
                .SetUpdate(true)
                .ToUniTask()
                .Forget();
            
            return sequence.ToUniTask();
        }

        private void ShowObjectsText(float value)
        {
            _menuLabel.color = new Color(1f, 1f, 1f, value);
        }

        public override UniTask UndoAnimate()
        {
            return UniTask.CompletedTask;
        }
    }
}