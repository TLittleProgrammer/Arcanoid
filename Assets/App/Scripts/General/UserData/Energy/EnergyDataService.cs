using System;
using App.Scripts.External.UserData;

namespace App.Scripts.General.UserData.Energy
{
    public sealed class EnergyDataService : IEnergyDataService
    {
        private readonly IUserDataContainer _userDataContainer;

        public event Action<int> EnergyWasChanged;

        public EnergyDataService(IUserDataContainer userDataContainer)
        {
            _userDataContainer = userDataContainer;
        }

        public void AddEnergy(int energy)
        {
            EnergyData energyData = (EnergyData)_userDataContainer.GetData<EnergyData>();

            energyData.EnergyCounter += energy;

            if (energyData.EnergyCounter < 0)
            {
                energyData.EnergyCounter = 0;
            }
            
            EnergyWasChanged?.Invoke(energyData.EnergyCounter);
            _userDataContainer.SaveData<EnergyData>();
        }

        public int GetEnergy()
        {
            return ((EnergyData)_userDataContainer.GetData<EnergyData>()).EnergyCounter;
        }
    }
}