using App.Scripts.External.Initialization;

namespace App.Scripts.General.Popup.AssetManagment
{
    public interface IPopupProvider : IAsyncInitializable<string>
    {
        IPopupView LoadPopup<TPopupView>() where TPopupView : IPopupView;
    }
}