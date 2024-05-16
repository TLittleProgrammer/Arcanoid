using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Healthes;
using App.Scripts.Scenes.GameScene.Popups;
using App.Scripts.Scenes.GameScene.Time;
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
        private readonly RootUIViewProvider _rootUIViewProvider;

        private bool _stateIsEntered = false;
        
        public GameLoopState(
            IEnumerable<ITickable> tickables,
            IHealthContainer healthContainer,
            IPopupService popupService,
            ITimeScaleAnimator timeScaleAnimator,
            IStateMachine stateMachine,
            RootUIViewProvider rootUIViewProvider)
        {
            _tickables = tickables;
            _healthContainer = healthContainer;
            _popupService = popupService;
            _timeScaleAnimator = timeScaleAnimator;
            _stateMachine = stateMachine;
            _rootUIViewProvider = rootUIViewProvider;
        }
        
        public void Enter()
        {
            _stateIsEntered = true;
            _healthContainer.LivesAreWasted += OnLivesAreWasted;
        }

        public void Exit()
        {
            _stateIsEntered = false;
            _healthContainer.LivesAreWasted -= OnLivesAreWasted;
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
            await _timeScaleAnimator.Animate(0f);
            _stateMachine.Enter<LooseState>();
                
            _popupService.Show<LoosePopupView>(_rootUIViewProvider.PopupUpViewProvider);
        }
    }
}