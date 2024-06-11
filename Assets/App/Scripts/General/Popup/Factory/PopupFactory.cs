using System.Linq;
using App.Scripts.General.Popup.AssetManagment;
using Cysharp.Threading.Tasks;
using Zenject;

namespace App.Scripts.General.Popup.Factory
{
    public sealed class PopupFactory : IPopupFactory
    {
        private readonly ViewPopupMapping _popupProvider;
        private DiContainer _diContainer;

        public PopupFactory(ViewPopupMapping popupProvider, DiContainer diContainer)
        {
            _popupProvider = popupProvider;
            _diContainer = diContainer;
        }

        public IPopupView Create<TViewPopupProvider>() where TViewPopupProvider : IPopupView
        {
            IPopupView prefab = _popupProvider.ViewPopupProviderMapping.First(x => x is TViewPopupProvider);

            return _diContainer.InstantiatePrefabForComponent<IPopupView>(prefab.GameObject);
        }
    }
}