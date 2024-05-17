using System;
using App.Scripts.External.Components;
using App.Scripts.External.Initialization;
using App.Scripts.General.Popup.AssetManagment;
using Cysharp.Threading.Tasks;
using Zenject;

namespace App.Scripts.General.Popup.Factory
{
    public sealed class PopupFactory : IPopupFactory, IAsyncInitializable<DiContainer>
    {
        private readonly IPopupProvider _popupProvider;
        private DiContainer _diContainer;

        public PopupFactory(IPopupProvider popupProvider, DiContainer diContainer)
        {
            _popupProvider = popupProvider;
            _diContainer = diContainer;
        }

        public async UniTask AsyncInitialize(DiContainer param)
        {
            _diContainer = param;
            await UniTask.CompletedTask;
        }

        public IPopupView Create<TViewPopupProvider>(ITransformable parent) where TViewPopupProvider : IPopupView
        {
            IPopupView prefab = _popupProvider.LoadPopup<TViewPopupProvider>();

            return _diContainer.InstantiatePrefabForComponent<IPopupView>(prefab.GameObject, parent.Transform);
        }
    }
}