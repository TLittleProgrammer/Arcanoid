using App.Scripts.External.UserData;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.InfoBetweenScenes;
using App.Scripts.General.UserData.Levels.Data;
using App.Scripts.Scenes.MainMenuScene.Command;
using App.Scripts.Scenes.MainMenuScene.MVVM.LevelPacks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Features.ActivateScreens
{
    public class ActivateScreensService : IInitializable
    {
        private readonly InfoBetweenScenes _infoBetweenScenes;
        private readonly Button _playButton;
        private readonly Button _backButton;
        private readonly GameObject _initialScreen;
        private readonly GameObject _levelPacks;
        private readonly Image _screenTransitionImage;
        private readonly ILoadLevelCommand _loadLevelCommand;
        private readonly LevelPackModel _levelPackModel;
        private readonly LevelPackProgressDictionary _levelPacksProgress;

        public ActivateScreensService(
            InfoBetweenScenes infoBetweenScenes,
            Button playButton,
            Button backButton,
            GameObject initialScreen,
            GameObject levelPacks,
            Image screenTransitionImage,
            IDataProvider<LevelPackProgressDictionary> levelPacksProgress,
            ILoadLevelCommand loadLevelCommand,
            LevelPackModel levelPackModel)
        {
            _infoBetweenScenes = infoBetweenScenes;
            _playButton = playButton;
            _backButton = backButton;
            _initialScreen = initialScreen;
            _levelPacks = levelPacks;
            _screenTransitionImage = screenTransitionImage;
            _loadLevelCommand = loadLevelCommand;
            _levelPackModel = levelPackModel;
            _levelPacksProgress = levelPacksProgress.GetData();
        }

        public void Initialize()
        {
            if (_infoBetweenScenes.NeedShowLevelPackContainer)
            {
                SwitchScreens(false, false);
            }
            else
            {
                SwitchScreens(true, false);
            }
            
            _playButton.onClick.AddListener(() =>
            {
                if (_levelPacksProgress.GetPassedLevelCount(0) == 0)
                {
                    _loadLevelCommand.Execute(_levelPackModel.GetFirstLevelItemData(), 0);
                }
                else
                {
                    SwitchScreens(false);
                }
            });
            
            _backButton.onClick.AddListener(() =>
            {
                SwitchScreens(true);
            });
        }

        private async void SwitchScreens(bool value, bool withAnimation = true)
        {
            if (withAnimation)
            {
                _screenTransitionImage.raycastTarget = true;
                await DOVirtual.Float(0f, 1f, 1f, UpdateFillAmount).ToUniTask();

                _initialScreen.gameObject.SetActive(value);
                _levelPacks.gameObject.SetActive(!value);

                await DOVirtual.Float(1f, 0f, 1f, UpdateFillAmount).ToUniTask();
                
                _screenTransitionImage.raycastTarget = false;
            }
            else
            {
                _initialScreen.gameObject.SetActive(value);
                _levelPacks.gameObject.SetActive(!value);
            }
        }

        private void UpdateFillAmount(float value)
        {
            _screenTransitionImage.fillAmount = value;
        }
    }
}