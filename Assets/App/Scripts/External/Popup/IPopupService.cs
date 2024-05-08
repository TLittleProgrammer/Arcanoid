using Cysharp.Threading.Tasks;

namespace App.Scripts.External.Popup
{
    public interface IPopupService
    {
        UniTask<IViewPopupProvider> Show<TPopup>() where TPopup : IViewPopupProvider;
        void Close<TPopup>() where TPopup : IViewPopupProvider;
    }
}