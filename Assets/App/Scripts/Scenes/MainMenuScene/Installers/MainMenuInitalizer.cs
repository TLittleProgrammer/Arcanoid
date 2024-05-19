using App.Scripts.General.Constants;
using App.Scripts.General.Energy;
using App.Scripts.General.Popup.AssetManagment;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Installers
{
    public class MainMenuInitalizer : IInitializable
    {
        private readonly IPopupProvider _popupProvider;
        private readonly EnergyScrollView _energyScrollView;
        private readonly EnergyService _energyService;

        public MainMenuInitalizer(IPopupProvider popupProvider, EnergyScrollView energyScrollView, EnergyService energyService)
        {
            _popupProvider = popupProvider;
            _energyScrollView = energyScrollView;
            _energyService = energyService;
        }
        
        public async void Initialize()
        {
            await _popupProvider.AsyncInitialize(Pathes.PathToPopups);
            _energyService.SetView(_energyScrollView);
        }
    }
}