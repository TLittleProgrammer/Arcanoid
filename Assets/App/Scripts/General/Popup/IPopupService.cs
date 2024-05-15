using App.Scripts.General.Components;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.Popup
{
    public interface IPopupService
    {
        IViewPopupProvider Show<TViewPopupProvider>(ITransformable parent = null) where TViewPopupProvider : IViewPopupProvider;
        UniTask Close<TPopup>() where TPopup : IViewPopupProvider;
    }
}