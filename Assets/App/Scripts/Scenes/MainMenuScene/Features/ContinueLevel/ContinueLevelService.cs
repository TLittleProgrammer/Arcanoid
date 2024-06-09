using App.Scripts.External.GameStateMachine;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.Constants;
using App.Scripts.General.Levels;
using App.Scripts.General.Levels.LevelPackInfoService;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.States;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Features.ContinueLevel
{
    public class ContinueLevelService : IInitializable
    {
        private readonly Button _playButton;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStateMachine _stateMachine;
        private readonly ILevelPackInfoService _levelPackInfoService;

        public ContinueLevelService(
            Button playButton,
            ISaveLoadService saveLoadService,
            IStateMachine stateMachine,
            ILevelPackInfoService levelPackInfoService)
        {
            _playButton = playButton;
            _saveLoadService = saveLoadService;
            _stateMachine = stateMachine;
            _levelPackInfoService = levelPackInfoService;
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

            _stateMachine.Enter<LoadingSceneState, string>(SceneNaming.Game);
        }
    }
}