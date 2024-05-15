using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.General.Popup
{
    public abstract class ViewPopupProvider : MonoBehaviour, IViewPopupProvider
    {
        public List<Button> Buttons { get; protected set; }

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
        
        public GameObject GameObject => gameObject;
    }
}