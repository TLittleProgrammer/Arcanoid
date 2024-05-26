using System.Collections.Generic;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Input;
using App.Scripts.Scenes.GameScene.Features.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.Load;
using App.Scripts.Scenes.GameScene.Features.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Pools;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Time;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint
{
    public class InitializeAllRestartableAnsTickablesLists : IInitializable
    {
        private readonly List<IRestartable> _generalRestartables;
        private readonly List<IRestartable> _restartablesForLoadNewLevel;
        private readonly List<ITickable> _gameLoopTickables;
        private readonly IHealthPointService _healthPointService;
        private readonly IHealthContainer _healthContainer;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IPlayerShapeMover _playerShapeMover;
        private readonly IBallMovementService _ballMovementService;
        private readonly IPopupService _popupService;
        private readonly IPoolContainer _poolContainer;
        private readonly ILevelLoader _levelLoader;
        private readonly ITimeProvider _timeProvider;
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly IClickDetector _clickDetector;
        private readonly IInputService _inputService;
        private readonly IBoostContainer _boostContainer;
        private readonly IBulletPositionChecker _bulletPositionChecker;

        public InitializeAllRestartableAnsTickablesLists(
            List<IRestartable> generalRestartables,
            List<IRestartable> restartablesForLoadNewLevel,
            List<ITickable> gameLoopTickables,
            IHealthPointService healthPointService,
            IHealthContainer healthContainer,
            ILevelProgressService levelProgressService,
            IPlayerShapeMover playerShapeMover,
            IBallMovementService ballMovementService,
            IPopupService popupService,
            IPoolContainer poolContainer,
            ILevelLoader levelLoader,
            ITimeProvider timeProvider,
            IGridPositionResolver gridPositionResolver,
            IClickDetector clickDetector,
            IInputService inputService,
            IBoostContainer boostContainer,
            IBulletPositionChecker bulletPositionChecker)
        {
            _generalRestartables = generalRestartables;
            _restartablesForLoadNewLevel = restartablesForLoadNewLevel;
            _gameLoopTickables = gameLoopTickables;
            _healthPointService = healthPointService;
            _healthContainer = healthContainer;
            _levelProgressService = levelProgressService;
            _playerShapeMover = playerShapeMover;
            _ballMovementService = ballMovementService;
            _popupService = popupService;
            _poolContainer = poolContainer;
            _levelLoader = levelLoader;
            _timeProvider = timeProvider;
            _gridPositionResolver = gridPositionResolver;
            _clickDetector = clickDetector;
            _inputService = inputService;
            _boostContainer = boostContainer;
            _bulletPositionChecker = bulletPositionChecker;
        }

        public void Initialize()
        {
            InitializeGeneralList();
            InitializeRestartablesListForNewLevel();
            InitializeTickablesList();
        }

        private void InitializeTickablesList()
        {
            _gameLoopTickables.Add(_playerShapeMover);
            _gameLoopTickables.Add(_clickDetector);
            _gameLoopTickables.Add(_inputService);
            _gameLoopTickables.Add(_ballMovementService);
        }

        private void InitializeRestartablesListForNewLevel()
        {
            _restartablesForLoadNewLevel.Add(_boostContainer);
            _restartablesForLoadNewLevel.Add(_bulletPositionChecker);
            _restartablesForLoadNewLevel.Add(_levelProgressService);
            _restartablesForLoadNewLevel.Add(_playerShapeMover);
            _restartablesForLoadNewLevel.Add(_ballMovementService);
            _restartablesForLoadNewLevel.Add(_poolContainer);
            _restartablesForLoadNewLevel.Add(_timeProvider);
            _restartablesForLoadNewLevel.Add(_popupService as IRestartable);
            _restartablesForLoadNewLevel.Add(_healthPointService);
            _restartablesForLoadNewLevel.Add(_healthContainer);
        }

        private void InitializeGeneralList()
        {
            _generalRestartables.Add(_boostContainer);
            _generalRestartables.Add(_bulletPositionChecker);
            _generalRestartables.Add(_gridPositionResolver);
            _generalRestartables.Add(_levelProgressService);
            _generalRestartables.Add(_playerShapeMover);
            _generalRestartables.Add(_ballMovementService);
            _generalRestartables.Add(_poolContainer);
            _generalRestartables.Add(_levelLoader);
            _generalRestartables.Add(_timeProvider);
            _generalRestartables.Add(_popupService as IRestartable);
            _generalRestartables.Add(_healthPointService);
            _generalRestartables.Add(_healthContainer);
        }
    }
}