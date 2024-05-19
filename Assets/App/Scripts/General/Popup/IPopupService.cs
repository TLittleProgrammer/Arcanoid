using App.Scripts.External.Components;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.Popup
{
    public interface IPopupService
    {
        IPopupView Show<TPopupView>(ITransformable parent = null) where TPopupView : IPopupView;
        UniTask Close<TPopup>() where TPopup : IPopupView;
        UniTask CloseAll();
    }
}