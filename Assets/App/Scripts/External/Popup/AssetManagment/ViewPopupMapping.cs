using System.Collections.Generic;
using App.Scripts.External.Popup;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.General.Popup.AssetManagment
{
    [CreateAssetMenu(menuName = "Configs/ViewPopupMapping", fileName = "ViewPopupMapping")]
    public class ViewPopupMapping : SerializedScriptableObject
    {
        public List<PopupView> ViewPopupProviderMapping;
    }
}