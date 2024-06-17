using App.Scripts.General.Animator;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.MVVM.Popups.Loose
{
    public class RestartPopupAnimator : MonoAnimator
    {
        [SerializeField] private Transform _popupTransform;
        [SerializeField] private Button[] _buttons;

        private void Awake()
        {
            foreach (Button button in _buttons)
            {
                button.transform.localScale = Vector3.zero;
            }
        }

        public override async UniTask Animate()
        {
            Sequence sequence = DOTween.Sequence();
            
            AnimatePopupTransform(Vector3.one, Vector3.zero).ToUniTask().Forget();

            foreach (Button button in _buttons)
            {
                AnimateButton(button, Vector3.one, 0.5f).SetEase(Ease.InOutBounce).ToUniTask().Forget();
            }

            await sequence
                .SetUpdate(true)
                .Play()
                .ToUniTask();
        }

        public override async UniTask UndoAnimate()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(AnimatePopupTransform(Vector3.zero, Vector3.one)).ToUniTask().Forget();

            await sequence
                .SetUpdate(true)
                .Play()
                .ToUniTask();
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> AnimateButton(Button button, Vector3 targetScale, float delay)
        {
            return button
                .transform
                .DOScale(targetScale, 0.35f)
                .SetUpdate(true)
                .SetDelay(delay);
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> AnimatePopupTransform(Vector3 to, Vector3 from)
        {
            return _popupTransform
                .DOScale(to, 0.35f)
                .SetUpdate(true)
                .From(from);
        }
    }
}