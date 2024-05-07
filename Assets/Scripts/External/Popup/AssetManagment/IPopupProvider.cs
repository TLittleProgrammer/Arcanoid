using Cysharp.Threading.Tasks;

namespace General.Popup.AssetManagment
{
    public interface IPopupProvider
    {
        UniTask<IViewPopupProvider> LoadPopup<TPopupProvider>() where TPopupProvider : IViewPopupProvider;
    }
}