using App.Scripts.External.Initialization;

namespace App.Scripts.General.Popup.AssetManagment
{
    public interface IPopupProvider : IAsyncInitializable<string>
    {
        IViewPopupProvider LoadPopup(PopupTypeId popupTypeId);
    }
}