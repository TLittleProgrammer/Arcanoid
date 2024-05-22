using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.General.Popup
{
    public interface IBackPopupPlane
    {
        public Image Image { get; }
        public Transform Transform { get; }
    }
}