using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Levels;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Grid;
using App.Scripts.Scenes.GameScene.Infrastructure;
using App.Scripts.Scenes.GameScene.LevelProgress;
using App.Scripts.Scenes.GameScene.Levels;
using App.Scripts.Scenes.GameScene.Levels.Load;
using App.Scripts.Scenes.GameScene.Levels.View;
using App.Scripts.Scenes.GameScene.LevelView;
using App.Scripts.Scenes.GameScene.Popups;
using App.Scripts.Scenes.GameScene.Time;
using Newtonsoft.Json;

namespace App.Scripts.Scenes.GameScene.States
{
    public class LoadNextLevelState : IState
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly IEnumerable<IRestartable> _restartables;
        private readonly IStateMachine _gameStateMachine;
        private readonly ILevelLoader _levelLoader;
        private readonly ILevelPackTransferData _levelPackTransferData;
        private readonly IPopupService _popupService;
        private readonly ILevelPackInfoView _levelPackInfoView;
        private readonly ITimeProvider _timeProvider;
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly ILevelProgressService _levelProgressService;
        private readonly ILevelViewUpdater _levelViewUpdater;

        public LoadNextLevelState(
            ILoadingScreen loadingScreen,
            IEnumerable<IRestartable> restartables,
            IStateMachine gameStateMachine,
            ILevelLoader levelLoader,
            ILevelPackTransferData levelPackTransferData,
            IPopupService popupService,
            ILevelPackInfoView levelPackInfoView,
            ITimeProvider timeProvider,
            IGridPositionResolver gridPositionResolver,
            ILevelProgressService levelProgressService)
        {
            _loadingScreen = loadingScreen;
            _restartables = restartables;
            _gameStateMachine = gameStateMachine;
            _levelLoader = levelLoader;
            _levelPackTransferData = levelPackTransferData;
            _popupService = popupService;
            _levelPackInfoView = levelPackInfoView;
            _timeProvider = timeProvider;
            _gridPositionResolver = gridPositionResolver;
            _levelProgressService = levelProgressService;
        }

        public async void Enter()
        {
            _loadingScreen.Show(false);

            foreach (IRestartable restartable in _restartables)
            {
                restartable.Restart();
            }

            await _popupService.Close<WinPopupView>();
            _timeProvider.TimeScale = 1f;

            _levelPackTransferData.LevelIndex++;
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(_levelPackTransferData.LevelPack.Levels[_levelPackTransferData.LevelIndex].text);

            await _gridPositionResolver.AsyncInitialize(levelData);
            _levelProgressService.CalculateStepByLevelData(levelData);
            
            _levelLoader.LoadLevel(levelData);
            _levelPackInfoView.UpdatePassedLevels(_levelPackTransferData.LevelIndex, _levelPackTransferData.LevelPack.Levels.Count);

            _gameStateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            _loadingScreen.Hide();
        }
    }
}