using App.Scripts.General.Constants;
using App.Scripts.General.Energy;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.Popup.AssetManagment;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Installers
{
    public class MainMenuInitalizer : IInitializable
    {
        private readonly IPopupProvider _popupProvider;
        private readonly EnergyView _energyView;
        private readonly EnergyViewModel _energyViewModel;

        public MainMenuInitalizer(
            IPopupProvider popupProvider,
            EnergyView energyView,
            EnergyViewModel energyViewModel)
        {
            _popupProvider = popupProvider;
            _energyView = energyView;
            _energyViewModel = energyViewModel;
        }
        
        public async void Initialize()
        {
            await _popupProvider.AsyncInitialize(Pathes.PathToPopups);
            _energyViewModel.AddView(_energyView);
        }
    }
}