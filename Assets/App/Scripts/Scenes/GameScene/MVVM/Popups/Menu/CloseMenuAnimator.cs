using App.Scripts.General.Animator;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.MVVM.Popups.Menu
{
    public class CloseMenuAnimator : MonoAnimator<ITweenersLocator>
    {
        [SerializeField] private Transform _menuView;
        
        public override UniTask Animate(ITweenersLocator tweenersLocator)
        {
            Sequence sequence = DOTween.Sequence();

            sequence
                .SetUpdate(true)
                .Append(AnimateScale(_menuView, Vector3.zero, 0.75f, Ease.InOutBounce));
            
            tweenersLocator.AddSequence(sequence);

            return sequence.ToUniTask();
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> AnimateScale(Transform transform, Vector3 targetScale, float duration, Ease ease)
        {
            return transform.DOScale(targetScale, duration).SetEase(ease);
        }

        public override UniTask UndoAnimate()
        {
            return UniTask.CompletedTask;
        }
    }
}