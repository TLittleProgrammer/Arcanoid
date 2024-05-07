using Cysharp.Threading.Tasks;
using UnityEngine;

namespace General.Popup
{
    public interface IViewPopupProvider
    {
        UniTask Show();
        UniTask Close();

        void LockButtons();
        void UnlockButtons();
    }

    public abstract class ViewPopupProvider : MonoBehaviour, IViewPopupProvider
    {
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
    }
}