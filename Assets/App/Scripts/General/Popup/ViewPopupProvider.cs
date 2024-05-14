using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.General.Popup
{
    public abstract class ViewPopupProvider : MonoBehaviour, IViewPopupProvider
    {
        [SerializeField] private PopupTypeId _popupTypeId;
        
        public PopupTypeId PopupTypeId
        {
            get => _popupTypeId;
            set => _popupTypeId = value;
        }

        public virtual async UniTask Show()
        {
            gameObject.SetActive(true);
            transform.DOScale(Vector3.one, 1f).From(Vector3.zero).ToUniTask().Forget();
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