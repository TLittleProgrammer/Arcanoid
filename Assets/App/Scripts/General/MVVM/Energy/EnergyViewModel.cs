using App.Scripts.General.Reactive;
using App.Scripts.General.UserData.Energy;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.MVVM.Energy
{
    public class EnergyViewModel : IInitializable
    {
        private readonly EnergyModel _energyModel;
        private readonly IEnergyDataService _energyDataService;

        public ReactiveProperty<int> SecondsToAddEnergy = new();
        public ReactiveProperty<int> CurrentEnergy = new();

        public EnergyViewModel(EnergyModel energyModel)
        {
            _energyModel = energyModel;
        }

        public int MaxEnergy => _energyModel.MaxEnergy;

        public void Initialize()
        {
            _energyModel.SecondsToAddEnergy.OnChanged += OnModelRemainingSecondsChanged;
            _energyModel.CurrentEnergy.OnChanged += OnModelCurrentEnergyChanged;

            CurrentEnergy.Value = _energyModel.CurrentEnergy.Value;
            SecondsToAddEnergy.Value = _energyModel.SecondsToAddEnergy.Value;
        }

        private void OnModelCurrentEnergyChanged(int energy)
        {
            CurrentEnergy.Value = energy;
        }

        private void OnModelRemainingSecondsChanged(int remainingSeconds)
        {
            SecondsToAddEnergy.Value = remainingSeconds;
        }
    }
}