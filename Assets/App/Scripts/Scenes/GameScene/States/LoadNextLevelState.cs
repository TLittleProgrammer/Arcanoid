using System.Collections.Generic;
using System.Threading.Tasks;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Levels;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.Popup;
using App.Scripts.Scenes.GameScene.Dotween;
using App.Scripts.Scenes.GameScene.Grid;
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
        private readonly IPopupService _popupService;
        private readonly ILevelPackInfoView _levelPackInfoView;
        private readonly ITimeProvider _timeProvider;
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly ILevelProgressService _levelProgressService;
        private readonly ITweenersLocator _tweenersLocator;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly ILevelViewUpdater _levelViewUpdater;
        
        private ILevelPackTransferData _levelPackTransferData;

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
            ILevelProgressService levelProgressService,
            ITweenersLocator tweenersLocator,
            ILevelPackInfoService levelPackInfoService)
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
            _tweenersLocator = tweenersLocator;
            _levelPackInfoService = levelPackInfoService;
        }

        public async void Enter()
        {
            await _loadingScreen.Show(false);

            RemoveTweeners();
            RestartAll();
            await ClosePopups();

            LevelData levelData = GetNextLevelData();

            await _gridPositionResolver.AsyncInitialize(levelData);
            _levelProgressService.CalculateStepByLevelData(levelData);
            
            _levelLoader.LoadLevel(levelData);
            _levelPackInfoView.Initialize(new LevelPackInfoRecord
            {
                AllLevelsCountFromPack = _levelPackTransferData.LevelPack.Levels.Count,
                CurrentLevelIndex = _levelPackTransferData.LevelIndex,
                Sprite = _levelPackTransferData.LevelPack.GalacticIcon,
                TargetScore = 0
            });
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        private LevelData GetNextLevelData()
        {
            _levelPackTransferData = _levelPackInfoService.UpdateLevelPackTransferData(_levelPackTransferData);
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(_levelPackTransferData.LevelPack.Levels[_levelPackTransferData.LevelIndex].text);

            return levelData;
        }

        private async Task ClosePopups()
        {
            await _popupService.Close<WinPopupView>();
            _timeProvider.TimeScale = 1f;
        }

        private void RemoveTweeners()
        {
            _tweenersLocator.RemoveAll();
        }

        private void RestartAll()
        {
            foreach (IRestartable restartable in _restartables)
            {
                restartable.Restart();
            }
        }

        public void Exit()
        {
            _loadingScreen.Hide();
        }
    }
}