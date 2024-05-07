using Cysharp.Threading.Tasks;
using UnityEngine;

namespace External.Popup.AssetManagment
{
    public sealed class ResourcesPopupProvider : IPopupProvider
    {
        private const string ViewPopupMappingPath = "";

        public async UniTask<IViewPopupProvider> LoadPopup<TPopupProvider>() where TPopupProvider : IViewPopupProvider
        {
            ViewPopupMapping viewPopupMapping = (ViewPopupMapping)await Resources.LoadAsync<ViewPopupMapping>(ViewPopupMappingPath);

            foreach (ViewPopupProvider provider in viewPopupMapping.ViewPopupProviderMapping)
            {
                if (provider is TPopupProvider)
                {
                    return provider;
                }
            }

            return null;
        }
    }
}