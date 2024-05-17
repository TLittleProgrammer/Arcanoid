using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.General.Popup.AssetManagment
{
    public sealed class ResourcesPopupProvider : IPopupProvider
    {
        private List<IPopupView> _mapping;

        public async UniTask AsyncInitialize(string path)
        {
            _mapping =
                new List<IPopupView>((
                        (ViewPopupMapping)await Resources.LoadAsync<ViewPopupMapping>(path))
                        .ViewPopupProviderMapping
                        .ToList()
                    );
        }

        public IPopupView LoadPopup<TPopupView>() where TPopupView : IPopupView
        {
            return _mapping.Find(x => x is TPopupView);
        }
    }
}