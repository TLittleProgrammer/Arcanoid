using App.Scripts.External.Components;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.Popup
{
    public interface IPopupService
    {
        TPopupView GetPopup<TPopupView>(ITransformable parent = null) where TPopupView : IPopupView;
        UniTask Close<TPopup>() where TPopup : IPopupView;
        UniTask CloseAll(bool fastClose = false);
    }
}