using App.Scripts.External.Initialization;
using Zenject;

namespace App.Scripts.General.Popup.Factory
{
    public interface IPopupFactory : IAsyncInitializable<DiContainer>
    {
        IPopupView Create<TPopupView>() where TPopupView : IPopupView;
    }
}