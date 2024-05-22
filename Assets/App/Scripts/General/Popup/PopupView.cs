using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.General.Popup
{
    public abstract class PopupView : MonoBehaviour, IPopupView
    {
        public virtual async UniTask Show()
        {
            gameObject.SetActive(true);
            await transform.DOScale(Vector3.one, 1f).From(Vector3.zero).ToUniTask();
        }

        public virtual async UniTask Close()
        {
            await transform.DOScale(Vector3.zero, 1f).From(Vector3.zero).ToUniTask();
            gameObject.SetActive(false);
        }
        
        public GameObject GameObject => gameObject;
    }
}