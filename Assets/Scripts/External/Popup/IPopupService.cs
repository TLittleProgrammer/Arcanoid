using Cysharp.Threading.Tasks;

namespace General.Popup
{
    public interface IPopupService
    {
        UniTask<IViewPopupProvider> Show<TPopup>() where TPopup : IViewPopupProvider;
        void Close<TPopup>() where TPopup : IViewPopupProvider;
    }
}