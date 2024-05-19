using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Healthes;
using App.Scripts.Scenes.GameScene.LevelProgress;
using App.Scripts.Scenes.GameScene.Popups;
using App.Scripts.Scenes.GameScene.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.States
{
    public class GameLoopState : IState, ITickable
    {
        private readonly IEnumerable<ITickable> _tickables;
        private readonly IHealthContainer _healthContainer;
        private readonly IPopupService _popupService;
        private readonly ITimeScaleAnimator _timeScaleAnimator;
        private readonly IStateMachine _stateMachine;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IBallFreeFlightMover _ballFreeFlightMover;
        private readonly RootUIViewProvider _rootUIViewProvider;

        private bool _stateIsEntered = false;
        private float _lastBallSpeed;
        
        public GameLoopState(
            IEnumerable<ITickable> tickables,
            IHealthContainer healthContainer,
            IPopupService popupService,
            ITimeScaleAnimator timeScaleAnimator,
            IStateMachine stateMachine,
            ILevelProgressService levelProgressService,
            IBallFreeFlightMover ballFreeFlightMover,
            RootUIViewProvider rootUIViewProvider)
        {
            _tickables = tickables;
            _healthContainer = healthContainer;
            _popupService = popupService;
            _timeScaleAnimator = timeScaleAnimator;
            _stateMachine = stateMachine;
            _levelProgressService = levelProgressService;
            _ballFreeFlightMover = ballFreeFlightMover;
            _rootUIViewProvider = rootUIViewProvider;
        }
        
        public void Enter()
        {
            _stateIsEntered = true;
            _ballFreeFlightMover.UpdateSpeed(_lastBallSpeed);
            _healthContainer.LivesAreWasted   += OnLivesAreWasted;
            _levelProgressService.LevelPassed += OnLevelPassed;
        }

        public void Exit()
        {
            _stateIsEntered = false;
            _lastBallSpeed = _ballFreeFlightMover.Speed;
            _ballFreeFlightMover.UpdateSpeed(-_lastBallSpeed);
            _healthContainer.LivesAreWasted   -= OnLivesAreWasted;
            _levelProgressService.LevelPassed -= OnLevelPassed;
        }

        public void Tick()
        {
            if (_stateIsEntered is false)
                return;

            foreach (ITickable tickable in _tickables)
            {
                tickable.Tick();
            }
        }

        private async void OnLivesAreWasted()
        {
            await AnimateTimeScaleTo(0f);
            
            _stateMachine.Enter<LooseState>();
            _popupService.Show<LoosePopupView>(_rootUIViewProvider.PopupUpViewProvider);
        }

        private async void OnLevelPassed()
        {
            await AnimateTimeScaleTo(0f);
            
            _stateMachine.Enter<WinState>();
        }

        private async UniTask AnimateTimeScaleTo(float to)
        {
            await _timeScaleAnimator.Animate(to);
        }
    }
}