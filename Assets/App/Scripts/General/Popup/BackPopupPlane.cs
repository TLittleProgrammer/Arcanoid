using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.General.Popup
{
    public class BackPopupPlane : MonoBehaviour, IBackPopupPlane
    {
        private Image _image;

        public Image Image => _image ??= GetComponent<Image>();
        public Transform Transform => transform;
    }
}