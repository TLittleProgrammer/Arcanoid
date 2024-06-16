using App.Scripts.General.Animator;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Popups.Animators
{
    public class ChangeIconPositions : MonoAnimator
    {
        [SerializeField] private RectTransform _topGalcticIcon;
        [SerializeField] private RectTransform _bottomGalcticIcon;
        
        public override UniTask Animate()
        {
            Vector2 firstAnchoredPosition = _topGalcticIcon.anchoredPosition;
            Vector2 secondAnchoredPosition = _bottomGalcticIcon.anchoredPosition;
            
            DOVirtual.Float(0f, 1f, 0.75f, value =>
            {
                _topGalcticIcon.anchoredPosition =
                    Vector2.Lerp(firstAnchoredPosition, new Vector2(-firstAnchoredPosition.x, firstAnchoredPosition.y), value);
                
                _bottomGalcticIcon.anchoredPosition =
                    Vector2.Lerp(secondAnchoredPosition, new Vector2(-secondAnchoredPosition.x, secondAnchoredPosition.y), value);
            });
            
            return UniTask.CompletedTask;
        }

        public override UniTask UndoAnimate()
        {
            return UniTask.CompletedTask;
        }
    }
}