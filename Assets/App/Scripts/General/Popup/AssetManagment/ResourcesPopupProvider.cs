using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.General.Popup.AssetManagment
{
    public sealed class ResourcesPopupProvider : IPopupProvider
    {
        private List<IViewPopupProvider> _mapping;

        public async UniTask AsyncInitialize(string path)
        {
            _mapping =
                new List<IViewPopupProvider>((
                        (ViewPopupMapping)await Resources.LoadAsync<ViewPopupMapping>(path))
                        .ViewPopupProviderMapping
                        .ToList()
                    );
        }

        public IViewPopupProvider LoadPopup<TViewPopupProvider>() where TViewPopupProvider : IViewPopupProvider
        {
            return _mapping.Find(x => x is TViewPopupProvider);
        }
    }
}