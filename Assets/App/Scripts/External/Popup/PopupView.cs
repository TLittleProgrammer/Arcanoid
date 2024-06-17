using App.Scripts.General.Popup;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.External.Popup
{
    public abstract class PopupView : MonoBehaviour, IPopupView
    {
        public virtual UniTask Show()
        {
            return UniTask.CompletedTask;
        }

        public virtual UniTask Close()
        {
            return UniTask.CompletedTask;
        }

        public GameObject GameObject => gameObject;
    }
}