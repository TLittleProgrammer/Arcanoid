using App.Scripts.General.Components;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.Popup
{
    public interface IPopupService
    {
        IViewPopupProvider Show(PopupTypeId popupTypeId, ITransformable parent = null);
        UniTask Close<TPopup>() where TPopup : IViewPopupProvider;
    }
}