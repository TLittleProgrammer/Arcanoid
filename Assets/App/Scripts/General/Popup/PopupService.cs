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
        private readonly List<IViewPopupProvider> _popupsList;

        public PopupService(IPopupFactory factory)
        {
            _factory = factory;
            _popupsList = new();
        }

        public IViewPopupProvider Show<TViewPopupProvider>(ITransformable parent = null) where TViewPopupProvider : IViewPopupProvider
        {
            IViewPopupProvider viewPopupProvider = _factory.Create<TViewPopupProvider>(parent);

            LockButtonsForLastPopup();
            
            _popupsList.Add(viewPopupProvider);
            viewPopupProvider.Show();
            
            return viewPopupProvider;
        }

        public async UniTask Close<TPopup>() where TPopup : IViewPopupProvider
        {
            IViewPopupProvider viewProvider = FindPopup<TPopup>();

            if (viewProvider is not null)
            {
                await viewProvider.Close();
                
                _popupsList.Remove(viewProvider);
                Object.Destroy(viewProvider.GameObject);
                
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

        private IViewPopupProvider FindPopup<TPopup>() where TPopup : IViewPopupProvider
        {
            foreach (IViewPopupProvider provider in _popupsList)
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
            foreach (IViewPopupProvider viewPopupProvider in _popupsList)
            {
                Object.Destroy(viewPopupProvider.GameObject);
            }
            
            _popupsList.Clear();
        }
    }
}