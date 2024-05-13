using System.Collections.Generic;
using App.Scripts.General.Components;
using App.Scripts.Scenes.GameScene.Infrastructure;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.General.Popup
{
    public sealed class PopupService : IPopupService, IRestartable
    {
        private readonly IViewPopupProvider.Factory _factory;
        private readonly List<IViewPopupProvider> _popupsList;

        public PopupService(IViewPopupProvider.Factory factory)
        {
            _factory = factory;
            _popupsList = new();
        }
        
        public IViewPopupProvider Show(PopupTypeId popupTypeId, ITransformable parent = null)
        {
            IViewPopupProvider viewPopupProvider = _factory.Create(popupTypeId, parent);

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
                _popupsList[^2].LockButtons();
            }
        }
        
        private void UnlockButtonsForLastViewProvider()
        {
            if (_popupsList.Count != 0)
            {
                _popupsList[^1].UnlockButtons();
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