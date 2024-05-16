using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Popup;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Constants;
using App.Scripts.Scenes.GameScene.Infrastructure;
using App.Scripts.Scenes.GameScene.Levels;
using App.Scripts.Scenes.GameScene.Popups;
using App.Scripts.Scenes.GameScene.States;
using App.Scripts.Scenes.GameScene.Time;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Healthes
{
    public sealed class HealthContainer : IHealthContainer
    {
        private readonly IHealthPointService _healthPointService;
        private readonly IPopupService _popupService;
        private readonly RootUIViewProvider _rootUIViewProvider;
        private readonly ITimeScaleAnimator _timeScaleAnimator;
        private readonly IStateMachine _gameStateMachine;

        private IEnumerable<IRestartable> _restartables;
        private int _currentHealthCounter;
        private int _maxHealthCounter;

        public HealthContainer(
            IHealthPointService healthPointService,
            IPopupService popupService,
            RootUIViewProvider rootUIViewProvider,
            ITimeScaleAnimator timeScaleAnimator,
            IStateMachine gameStateMachine)
        {
            _healthPointService = healthPointService;
            _popupService = popupService;
            _rootUIViewProvider = rootUIViewProvider;
            _timeScaleAnimator = timeScaleAnimator;
            _gameStateMachine = gameStateMachine;
        }

        public async UniTask AsyncInitialize(LevelData param, IEnumerable<IRestartable> restartables)
        {
            _currentHealthCounter = _maxHealthCounter = param.HealthCount == 0 ? GameConstants.DefaultHealthCount : param.HealthCount;
            _restartables = restartables;
            await UniTask.CompletedTask;
        }

        public async void UpdateHealth(int healthCount)
        {
            if (healthCount < 0)
            {
                foreach (IRestartable restartable in _restartables)
                {
                    restartable.Restart();
                }
            }
            
            _healthPointService.UpdateHealth(healthCount);
            
            if (_currentHealthCounter + healthCount <= 0)
            {
                _currentHealthCounter = 0;

                _gameStateMachine.Enter<LooseState>();
                await _timeScaleAnimator.Animate(0f);
                
                _popupService.Show<LoosePopupPopupPopupPopupView>(_rootUIViewProvider.PopupUpViewProvider);
            }
            else
            {
                _currentHealthCounter += healthCount;
            }
        }

        public void Restart()
        {
            _currentHealthCounter = _maxHealthCounter;
        }
    }
}