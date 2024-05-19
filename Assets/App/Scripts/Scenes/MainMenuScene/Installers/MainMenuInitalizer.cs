using App.Scripts.General.Constants;
using App.Scripts.General.Popup.AssetManagment;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Installers
{
    public class MainMenuInitalizer : IInitializable
    {
        private readonly IPopupProvider _popupProvider;

        public MainMenuInitalizer(IPopupProvider popupProvider)
        {
            _popupProvider = popupProvider;
        }
        
        public async void Initialize()
        {
            await _popupProvider.AsyncInitialize(Pathes.PathToPopups);
        }
    }
}