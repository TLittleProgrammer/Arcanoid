namespace App.Scripts.General.Popup.Factory
{
    public interface IPopupFactory
    {
        IPopupView Create<TPopupView>() where TPopupView : IPopupView;
    }
}