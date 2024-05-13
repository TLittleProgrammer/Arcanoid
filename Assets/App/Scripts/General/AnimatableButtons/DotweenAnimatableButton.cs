using App.Scripts.External.GameStateMachine;
using App.Scripts.General.AnimatableButtons.Settings;
using App.Scripts.General.DotweenContainerService;
using App.Scripts.General.States;
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
        private IStateMachine _stateMachine;

        [Inject]
        private void Construct(IDotweenContainerService dotweenContainerService, IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _dotweenContainerService = dotweenContainerService;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_stateMachine.CurrentState is not LoadingSceneState)
            {
                Play(_animationSettings.TargetScale);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_stateMachine.CurrentState is not LoadingSceneState)
            {
                Stop();
                Play(Vector3.one);
            }
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