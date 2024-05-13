using App.Scripts.External.Initialization;
using App.Scripts.General.Components;
using App.Scripts.General.Popup.AssetManagment;
using Cysharp.Threading.Tasks;
using Zenject;

namespace App.Scripts.General.Popup.Factory
{
    public sealed class PopupFactory : IFactory<PopupTypeId, ITransformable, IViewPopupProvider>, IAsyncInitializable<DiContainer>
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

        public IViewPopupProvider Create(PopupTypeId param, ITransformable parent)
        {
            IViewPopupProvider prefab = _popupProvider.LoadPopup(param);

            return _diContainer.InstantiatePrefabForComponent<IViewPopupProvider>(prefab.GameObject, parent.Transform);
        }
    }
}