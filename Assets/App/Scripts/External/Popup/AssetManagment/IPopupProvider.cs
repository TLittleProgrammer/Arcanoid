using Cysharp.Threading.Tasks;

namespace App.Scripts.External.Popup.AssetManagment
{
    public interface IPopupProvider
    {
        UniTask<IViewPopupProvider> LoadPopup<TPopupProvider>() where TPopupProvider : IViewPopupProvider;
    }
}