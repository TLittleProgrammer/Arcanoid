using System.Collections.Generic;
using System.Threading.Tasks;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.UserData;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.Levels;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.Popup;
using App.Scripts.General.UserData.Data;
using App.Scripts.General.UserData.Services;
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
        private readonly ILevelPackTransferData _levelPackTransferData;
        private readonly IPopupService _popupService;
        private readonly ILevelPackInfoView _levelPackInfoView;
        private readonly ITimeProvider _timeProvider;
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly ILevelProgressService _levelProgressService;
        private readonly ITweenersLocator _tweenersLocator;
        private readonly LevelProgressDataService _levelProgressDataService;
        private readonly IUserDataContainer _userDataContainer;
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
            ILevelProgressService levelProgressService,
            ITweenersLocator tweenersLocator,
            LevelProgressDataService levelProgressDataService,
            IUserDataContainer userDataContainer)
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
            _levelProgressDataService = levelProgressDataService;
            _userDataContainer = userDataContainer;
        }

        public async void Enter()
        {
            await _loadingScreen.Show(false);

            RemoveTweeners();
            RestartAll();
            await ClosePopups();
            UpdateUserData();

            _levelPackTransferData.LevelIndex++;
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(_levelPackTransferData.LevelPack.Levels[_levelPackTransferData.LevelIndex].text);

            await _gridPositionResolver.AsyncInitialize(levelData);
            _levelProgressService.CalculateStepByLevelData(levelData);
            
            _levelLoader.LoadLevel(levelData);
            _levelPackInfoView.UpdatePassedLevels(_levelPackTransferData.LevelIndex, _levelPackTransferData.LevelPack.Levels.Count);

            _gameStateMachine.Enter<GameLoopState>();
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

        private void UpdateUserData()
        { 
            _levelProgressDataService.PassLevel(_levelPackTransferData.PackIndex);
            _userDataContainer.SaveData<LevelPackProgressDictionary>();
        }

        public void Exit()
        {
            _loadingScreen.Hide();
        }
    }
}