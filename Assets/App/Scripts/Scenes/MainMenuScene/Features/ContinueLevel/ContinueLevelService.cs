using System;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.Constants;
using App.Scripts.General.Energy;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Levels;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.States;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.ContinueLevel
{
    public class ContinueLevelService : IInitializable
    {
        private readonly Button _playButton;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStateMachine _stateMachine;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly EnergyViewModel _energyViewModel;

        public ContinueLevelService(
            Button playButton,
            ISaveLoadService saveLoadService,
            IStateMachine stateMachine,
            ILevelPackInfoService levelPackInfoService,
            EnergyViewModel energyViewModel)
        {
            _playButton = playButton;
            _saveLoadService = saveLoadService;
            _stateMachine = stateMachine;
            _levelPackInfoService = levelPackInfoService;
            _energyViewModel = energyViewModel;
        }

        public void Initialize()
        {
            if (_saveLoadService.Exists("currentLevelProgress.json"))
            {
                _playButton.onClick.AddListener(LoadSavedLevel);
            }
            else
            {
                _playButton.transform.parent.gameObject.SetActive(false);
            }
        }

        private void LoadSavedLevel()
        {
            _levelPackInfoService.LevelPackTransferData =
                new LevelPackTransferData
                {
                    NeedContinue = true
                };

            _energyViewModel.Dispose();

            _stateMachine.Enter<LoadingSceneState, string>(SceneNaming.Game);
        }
    }
}