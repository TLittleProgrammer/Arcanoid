using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.General.Popup
{
    public abstract class ViewPopupProvider : MonoBehaviour, IViewPopupProvider
    {
        public PopupTypeId PopupTypeId { get; set; }

        public virtual async UniTask Show()
        {
            gameObject.SetActive(true);
            await UniTask.CompletedTask;
        }

        public virtual async UniTask Close()
        {
            gameObject.SetActive(false);
            await UniTask.CompletedTask;
        }

        public abstract void LockButtons();
        public abstract void UnlockButtons();
        public GameObject GameObject => gameObject;
    }
}