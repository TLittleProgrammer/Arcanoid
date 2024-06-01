using System;

namespace App.Scripts.General.UserData.Energy
{
    public interface IEnergyDataService
    {
        int CurrentValue { get; }
        
        event Action<int> ValueChanged;
        void Add(int value);
        void AddEnergyByPassedLevel();
    }
}