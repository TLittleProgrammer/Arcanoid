using App.Scripts.General.Components;

namespace App.Scripts.General.Popup.Factory
{
    public interface IPopupFactory
    {
        IViewPopupProvider Create<TViewPopupProvider>(ITransformable parent) where TViewPopupProvider : IViewPopupProvider;
    }
}