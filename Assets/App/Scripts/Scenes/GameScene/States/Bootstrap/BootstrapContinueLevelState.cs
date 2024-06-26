﻿using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.UserData;
using App.Scripts.Scenes.GameScene.Features.Entities.Walls;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.States.Gameloop;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapContinueLoadLevelState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly List<IInitializeByLevelProgress> _initializeByLevelProgresses;
        private readonly IDataProvider<LevelDataProgress> _dataProvider;
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly ILevelProgressService _levelProgressService;
        private readonly IWallLoader _wallLoader;

        public BootstrapContinueLoadLevelState(
            IStateMachine stateMachine,
            List<IInitializeByLevelProgress> initializeByLevelProgresses,
            IDataProvider<LevelDataProgress> dataProvider,
            IGridPositionResolver gridPositionResolver,
            ILevelProgressService levelProgressService,
            IWallLoader wallLoader)
        {
            _stateMachine = stateMachine;
            _initializeByLevelProgresses = initializeByLevelProgresses;
            _dataProvider = dataProvider;
            _gridPositionResolver = gridPositionResolver;
            _levelProgressService = levelProgressService;
            _wallLoader = wallLoader;
        }
        
        public async UniTask Enter()
        {
            LevelDataProgress levelDataProgress = _dataProvider.GetData();
            
            await _gridPositionResolver.AsyncInitialize(levelDataProgress.LevelData);

            foreach (IInitializeByLevelProgress progress in _initializeByLevelProgresses)
            {
                progress.LoadProgress(levelDataProgress);
            }
            
            _levelProgressService.Initialize();
            
            await _wallLoader.AsyncInitialize();

            _stateMachine.Enter<GameLoopState>().Forget();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}