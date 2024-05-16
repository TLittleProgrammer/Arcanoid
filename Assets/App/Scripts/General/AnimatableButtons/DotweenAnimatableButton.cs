using App.Scripts.General.AnimatableButtons.Settings;
using App.Scripts.General.DotweenContainerService;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace App.Scripts.General.AnimatableButtons
{
    public class DotweenAnimatableButton : MonoBehaviour, IAnimatableButton<Vector3>, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private DotweenAnimatableButtonSettings _animationSettings;
        [SerializeField] private Transform _animatedTransform;
        
        private IDotweenContainerService _dotweenContainerService;
        private Sequence _sequence;

        [Inject]
        private void Construct(IDotweenContainerService dotweenContainerService)
        {
            _dotweenContainerService = dotweenContainerService;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Play(_animationSettings.TargetScale);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Stop();
            Play(Vector3.one);
        }

        public void Play(Vector3 targetScale)
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(
                _animatedTransform
                    .DOScale(targetScale, _animationSettings.Duration)
                    .SetEase(_animationSettings.Ease)
                );
            
            _dotweenContainerService.AddTween(_sequence);
        }

        public void Stop()
        {
            _dotweenContainerService.RemoveTween(_sequence);
        }
    }
}