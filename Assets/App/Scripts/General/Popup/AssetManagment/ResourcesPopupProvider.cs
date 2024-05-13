using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.Initialization;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.General.Popup.AssetManagment
{
    public sealed class ResourcesPopupProvider : IPopupProvider
    {
        private Dictionary<PopupTypeId, IViewPopupProvider> _mapping;

        public async UniTask AsyncInitialize(string path)
        {
            _mapping =
                ((ViewPopupMapping)await Resources.LoadAsync<ViewPopupMapping>(path))
                    .ViewPopupProviderMapping
                    .ToDictionary(x => x.PopupTypeId, x => x as IViewPopupProvider);
        }

        public IViewPopupProvider LoadPopup(PopupTypeId popupTypeId)
        {
            return _mapping.TryGetValue(popupTypeId, out IViewPopupProvider viewPopupProvider)
                ? viewPopupProvider
                : null;
        }
    }
}