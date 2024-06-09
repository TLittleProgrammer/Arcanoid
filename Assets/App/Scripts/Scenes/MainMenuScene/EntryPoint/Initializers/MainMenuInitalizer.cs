using App.Scripts.General.Constants;
using App.Scripts.General.Energy;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.Popup.AssetManagment;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.EntryPoint.Initializers
{
    public class MainMenuInitalizer : IInitializable
    {
        private readonly EnergyView _energyView;
        private readonly EnergyViewModel _energyViewModel;

        public MainMenuInitalizer(EnergyView energyView, EnergyViewModel energyViewModel)
        {
            _energyView = energyView;
            _energyViewModel = energyViewModel;
        }
        
        public void Initialize()
        {
            _energyView.Initialize(_energyViewModel);
        }
    }
}