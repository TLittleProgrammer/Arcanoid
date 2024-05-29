using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Helpers;
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
        private readonly RootUIViewProvider _rootUIViewProvider;
        private readonly IServicesActivator _servicesActivator;
        private readonly GameLoopSubscriber _gameLoopSubscriber;
        private readonly IBallsService _ballsService;

        public bool IsActive { get; set; }

        private float _lastSpeedMultiplier;
        
        public GameLoopState(
            IEnumerable<ITickable> tickables,
            IHealthContainer healthContainer,
            IPopupService popupService,
            ITimeScaleAnimator timeScaleAnimator,
            IStateMachine stateMachine,
            ILevelProgressService levelProgressService,
            RootUIViewProvider rootUIViewProvider,
            IServicesActivator servicesActivator,
            GameLoopSubscriber gameLoopSubscriber,
            IBallsService ballsService)
        {
            
            _tickables = tickables;
            _healthContainer = healthContainer;
            _popupService = popupService;
            _timeScaleAnimator = timeScaleAnimator;
            _stateMachine = stateMachine;
            _levelProgressService = levelProgressService;
            _rootUIViewProvider = rootUIViewProvider;
            _servicesActivator = servicesActivator;
            _gameLoopSubscriber = gameLoopSubscriber;
            _ballsService = ballsService;

            _lastSpeedMultiplier = 1f;
        }

        public async UniTask Enter()
        {
            _servicesActivator.SetActiveToServices(true);

            _gameLoopSubscriber.SubscribeAll();
            _healthContainer.LivesAreWasted   += OnLivesAreWasted;
            _levelProgressService.LevelPassed += OnLevelPassed;
            
            _ballsService.SetSpeedMultiplier(_lastSpeedMultiplier);
            
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            _servicesActivator.SetActiveToServices(false);
            
            _gameLoopSubscriber.DescribeAll();
            _healthContainer.LivesAreWasted   -= OnLivesAreWasted;
            _levelProgressService.LevelPassed -= OnLevelPassed;

            _lastSpeedMultiplier = _ballsService.SpeedMultiplier;
            _ballsService.SetSpeedMultiplier(0f);

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