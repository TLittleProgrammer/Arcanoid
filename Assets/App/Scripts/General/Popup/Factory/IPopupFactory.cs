using App.Scripts.External.Components;

namespace App.Scripts.General.Popup.Factory
{
    public interface IPopupFactory
    {
        IPopupView Create<TPopupView>(ITransformable parent) where TPopupView : IPopupView;
    }
}