using App.Scripts.General.Components;

namespace App.Scripts.General.Popup.Factory
{
    public interface IPopupFactory
    {
        IPopupView Create<TPopupView>(ITransformable parent) where TPopupView : IPopupView;
    }
}