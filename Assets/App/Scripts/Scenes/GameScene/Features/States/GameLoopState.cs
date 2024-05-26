using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.MiniGun;
using App.Scripts.Scenes.GameScene.Features.Popups;
using App.Scripts.Scenes.GameScene.Features.Time;
using Cysharp.Threading.Tasks;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.States
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
        private readonly IBoostContainer _boostContainer;
        private readonly IBoostMoveService _boostMoveService;
        private readonly IMiniGunService _miniGunService;

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
            RootUIViewProvider rootUIViewProvider,
            IBoostContainer boostContainer,
            IBoostMoveService boostMoveService,
            IMiniGunService miniGunService)
        {
            _tickables = tickables;
            _healthContainer = healthContainer;
            _popupService = popupService;
            _timeScaleAnimator = timeScaleAnimator;
            _stateMachine = stateMachine;
            _levelProgressService = levelProgressService;
            _ballFreeFlightMover = ballFreeFlightMover;
            _rootUIViewProvider = rootUIViewProvider;
            _boostContainer = boostContainer;
            _boostMoveService = boostMoveService;
            _miniGunService = miniGunService;
        }
        
        public async UniTask Enter()
        {
            _boostMoveService.IsActive = true;
            _boostContainer.IsActive = true;
            _miniGunService.IsActive = true;
            _stateIsEntered = true;
            _ballFreeFlightMover.Continue();
            
            _healthContainer.LivesAreWasted   += OnLivesAreWasted;
            _levelProgressService.LevelPassed += OnLevelPassed;
            
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            _boostMoveService.IsActive = false;
            _boostContainer.IsActive = false;
            _miniGunService.IsActive = false;
            _stateIsEntered = false;
            _lastBallSpeed = _ballFreeFlightMover.VelocitySpeed;
            _ballFreeFlightMover.Reset();
            
            _healthContainer.LivesAreWasted   -= OnLivesAreWasted;
            _levelProgressService.LevelPassed -= OnLevelPassed;
            
            await UniTask.CompletedTask;
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