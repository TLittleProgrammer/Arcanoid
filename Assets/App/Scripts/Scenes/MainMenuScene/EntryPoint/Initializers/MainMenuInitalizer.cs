using App.Scripts.General.Constants;
using App.Scripts.General.Energy;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Installers
{
    public class MainMenuInitalizer : IInitializable
    {
        private readonly IPopupProvider _popupProvider;
        private readonly EnergyView _energyView;
        private readonly IEnergyService _energyService;

        public MainMenuInitalizer(
            IPopupProvider popupProvider,
            EnergyView energyView,
            IEnergyService energyService)
        {
            _popupProvider = popupProvider;
            _energyView = energyView;
            _energyService = energyService;
        }
        
        public async void Initialize()
        {
            await _popupProvider.AsyncInitialize(Pathes.PathToPopups);
            _energyService.AddView(_energyView);
        }
    }
}