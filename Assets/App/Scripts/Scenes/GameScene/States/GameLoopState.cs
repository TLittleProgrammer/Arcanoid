using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.ServiceActivator;
using App.Scripts.Scenes.GameScene.Features.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.States
{
    public class GameLoopState : IState, ITickable, IActivable
    {
        private readonly IEnumerable<ITickable> _tickables;
        private readonly IHealthContainer _healthContainer;
        private readonly IPopupService _popupService;
        private readonly ITimeScaleAnimator _timeScaleAnimator;
        private readonly IStateMachine _stateMachine;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IBallFreeFlightMover _ballFreeFlightMover;
        private readonly RootUIViewProvider _rootUIViewProvider;
        private readonly IServicesActivator _servicesActivator;

        public bool IsActive { get; set; }

        public GameLoopState(
            IEnumerable<ITickable> tickables,
            IHealthContainer healthContainer,
            IPopupService popupService,
            ITimeScaleAnimator timeScaleAnimator,
            IStateMachine stateMachine,
            ILevelProgressService levelProgressService,
            IBallFreeFlightMover ballFreeFlightMover,
            RootUIViewProvider rootUIViewProvider,
            IServicesActivator servicesActivator)
        {
            
            _tickables = tickables;
            _healthContainer = healthContainer;
            _popupService = popupService;
            _timeScaleAnimator = timeScaleAnimator;
            _stateMachine = stateMachine;
            _levelProgressService = levelProgressService;
            _ballFreeFlightMover = ballFreeFlightMover;
            _rootUIViewProvider = rootUIViewProvider;
            _servicesActivator = servicesActivator;
        }

        public async UniTask Enter()
        {
            _servicesActivator.SetActiveToServices(true);
            _ballFreeFlightMover.Continue();
            
            _healthContainer.LivesAreWasted   += OnLivesAreWasted;
            _levelProgressService.LevelPassed += OnLevelPassed;
            
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            _servicesActivator.SetActiveToServices(false);
            _ballFreeFlightMover.Reset();
            
            _healthContainer.LivesAreWasted   -= OnLivesAreWasted;
            _levelProgressService.LevelPassed -= OnLevelPassed;
            
            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            if (IsActive is false)
                return;

            foreach (ITickable tickable in _tickables)
            {
                tickable.Tick();
            }
        }

        private void OnLivesAreWasted()
        {
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