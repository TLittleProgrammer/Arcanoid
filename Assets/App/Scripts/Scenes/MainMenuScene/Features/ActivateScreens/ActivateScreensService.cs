using App.Scripts.General.InfoBetweenScenes;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        private readonly Image _screenTransitionImage;

        public ActivateScreensService(
            InfoBetweenScenes infoBetweenScenes,
            Button playButton,
            Button backButton,
            GameObject initialScreen,
            GameObject levelPacks,
            Image screenTransitionImage)
        {
            _infoBetweenScenes = infoBetweenScenes;
            _playButton = playButton;
            _backButton = backButton;
            _initialScreen = initialScreen;
            _levelPacks = levelPacks;
            _screenTransitionImage = screenTransitionImage;
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
                SwitchScreens(false);
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