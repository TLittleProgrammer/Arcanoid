using System;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.Constants;
using App.Scripts.General.Energy;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Levels;
using App.Scripts.General.States;
using UnityEngine;
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
        private readonly IEnergyService _energyService;

        public ContinueLevelService(
            Button playButton,
            ISaveLoadService saveLoadService,
            IStateMachine stateMachine,
            ILevelPackInfoService levelPackInfoService,
            IEnergyService energyService)
        {
            _playButton = playButton;
            _saveLoadService = saveLoadService;
            _stateMachine = stateMachine;
            _levelPackInfoService = levelPackInfoService;
            _energyService = energyService;
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
            _levelPackInfoService.SetData(new LevelPackTransferData
            {
                NeedContinue = true
            });

            _energyService.Dispose();

            _stateMachine.Enter<LoadingSceneState, string, bool, Action>(SceneNaming.Game, false, null);
        }
    }
}