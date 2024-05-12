using Cysharp.Threading.Tasks;

namespace App.Scripts.External.Popup
{
    public interface IViewPopupProvider
    {
        UniTask Show();
        UniTask Close();

        void LockButtons();
        void UnlockButtons();
    }
}