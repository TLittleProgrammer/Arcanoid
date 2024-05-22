using App.Scripts.General.InfoBetweenScenes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.ActivateScreens
{
    public class ActivateScreensService : IInitializable
    {
        private readonly InfoBetweenScenes _infoBetweenScenes;
        private readonly Button _playButton;
        private readonly Button _backButton;
        private readonly GameObject _initialScreen;
        private readonly GameObject _levelPacks;

        public ActivateScreensService(
            InfoBetweenScenes infoBetweenScenes,
            Button playButton,
            Button backButton,
            GameObject initialScreen,
            GameObject levelPacks)
        {
            _infoBetweenScenes = infoBetweenScenes;
            _playButton = playButton;
            _backButton = backButton;
            _initialScreen = initialScreen;
            _levelPacks = levelPacks;
        }

        public void Initialize()
        {
            if (_infoBetweenScenes.NeedShowLevelPackContainer)
            {
                SwitchScreens(false);
            }
            else
            {
                SwitchScreens(true);
            }
            
            _playButton.onClick.AddListener(() =>
            {
                SwitchScreens(false);
            });
            
            _backButton.onClick.AddListener(() =>
            {
                SwitchScreens(true);
            });
        }

        private void SwitchScreens(bool value)
        {
            _initialScreen.gameObject.SetActive(value);
            _levelPacks.gameObject.SetActive(!value);
        }
    }
}