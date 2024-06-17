using App.Scripts.General.Animator;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Scenes.GameScene.Features.Popups.Animators
{
    public class CircleAnimator : MonoAnimator<ITweenersLocator>
    {
        [SerializeField] private Image _circleImage;
        
        public override UniTask Animate(ITweenersLocator tweenersLocator)
        {
            var rotateTweener = _circleImage
                .transform
                .DORotate(new Vector3(0f, 0f, -360f), 4f, RotateMode.WorldAxisAdd)
                .SetRelative()
                .SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear);

            var scaleTweener = _circleImage
                .transform
                .DOScale(Vector3.one * 1.25f, 1f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);
            
            
            tweenersLocator.AddTweener(rotateTweener);
            tweenersLocator.AddTweener(scaleTweener);
            
            return UniTask.CompletedTask;
        }

        public override UniTask UndoAnimate()
        {
            return UniTask.CompletedTask;
        }
    }
}