using System.Collections.Generic;
using App.Scripts.General.Components;
using App.Scripts.General.Popup.Factory;
using App.Scripts.Scenes.GameScene.Infrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.General.Popup
{
    public sealed class PopupService : IPopupService, IRestartable
    {
        private readonly IPopupFactory _factory;
        private readonly List<IPopupView> _popupsList;

        public PopupService(IPopupFactory factory)
        {
            _factory = factory;
            _popupsList = new();
        }

        public IPopupView Show<TPopupView>(ITransformable parent = null) where TPopupView : IPopupView
        {
            IPopupView popupView = _factory.Create<TPopupView>(parent);

            LockButtonsForLastPopup();
            
            _popupsList.Add(popupView);
            popupView.Show();
            
            return popupView;
        }

        public async UniTask Close<TPopup>() where TPopup : IPopupView
        {
            IPopupView popupViewProvider = FindPopup<TPopup>();

            if (popupViewProvider is not null)
            {
                await popupViewProvider.Close();
                
                _popupsList.Remove(popupViewProvider);
                Object.Destroy(popupViewProvider.GameObject);
                
                UnlockButtonsForLastViewProvider();
            }

            await UniTask.CompletedTask;
        }

        private void LockButtonsForLastPopup()
        {
            if (_popupsList.Count > 1)
            {
                UpdateInteractableForButtons(_popupsList[^2].Buttons.ToArray(), false);
            }
        }
        
        private void UnlockButtonsForLastViewProvider()
        {
            if (_popupsList.Count != 0)
            {
                UpdateInteractableForButtons(_popupsList[^1].Buttons.ToArray(), true);
            }
        }

        private void UpdateInteractableForButtons(Button[] buttons, bool interactableValue)
        {
            foreach (Button button in buttons)
            {
                button.interactable = interactableValue;
            }
        }

        private IPopupView FindPopup<TPopup>() where TPopup : IPopupView
        {
            foreach (IPopupView provider in _popupsList)
            {
                if (provider is TPopup)
                {
                    return provider;
                }
            }

            return null;
        }

        public void Restart()
        {
            foreach (IPopupView viewPopupProvider in _popupsList)
            {
                Object.Destroy(viewPopupProvider.GameObject);
            }
            
            _popupsList.Clear();
        }
    }
}