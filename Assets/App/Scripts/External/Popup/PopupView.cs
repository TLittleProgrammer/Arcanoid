using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.General.Popup
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