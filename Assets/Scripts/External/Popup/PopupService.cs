using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using General.Popup.AssetManagment;

namespace General.Popup
{
    public sealed class PopupService : IPopupService
    {
        private readonly IPopupProvider _provider;
        private readonly List<IViewPopupProvider> _popupsList;

        public PopupService(IPopupProvider provider)
        {
            _provider = provider;
            _popupsList = new();
        }
        
        public async UniTask<IViewPopupProvider> Show<TPopup>() where TPopup : IViewPopupProvider
        {
            IViewPopupProvider viewPopupProvider = await _provider.LoadPopup<TPopup>();

            LockButtonsForLastPopup();
            _popupsList.Add(viewPopupProvider);
            
            return viewPopupProvider;
        }

        public async void Close<TPopup>() where TPopup : IViewPopupProvider
        {
            IViewPopupProvider viewProvider = FindPopup<TPopup>();

            if (viewProvider is not null)
            {
                await viewProvider.Close();
                _popupsList.Remove(viewProvider);
                
                UnlockButtonsForLastViewProvider();
            }
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
    }
}