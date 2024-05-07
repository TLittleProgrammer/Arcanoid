using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace External.Popup.AssetManagment
{
    [CreateAssetMenu(menuName = "Configs/ViewPopupMapping", fileName = "ViewPopupMapping")]
    public class ViewPopupMapping : SerializedScriptableObject
    {
        public List<IViewPopupProvider> ViewPopupProviderMapping;
    }
}