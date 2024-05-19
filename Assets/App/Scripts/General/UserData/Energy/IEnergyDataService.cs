using System;

namespace App.Scripts.General.UserData.Energy
{
    public interface IEnergyDataService
    {
        event Action<int> EnergyWasChanged;
        void AddEnergy(int energy);
        int GetEnergy();
    }
}