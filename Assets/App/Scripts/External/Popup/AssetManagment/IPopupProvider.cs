using Cysharp.Threading.Tasks;

namespace External.Popup.AssetManagment
{
    public interface IPopupProvider
    {
        UniTask<IViewPopupProvider> LoadPopup<TPopupProvider>() where TPopupProvider : IViewPopupProvider;
    }
}