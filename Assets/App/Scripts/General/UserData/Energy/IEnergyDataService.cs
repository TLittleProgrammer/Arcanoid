using System;
using App.Scripts.External.Initialization;

namespace App.Scripts.General.UserData.Energy
{
    public interface IEnergyDataService : IAsyncInitializable
    {
        event Action<int> ValueChanged;
        void Add(int value);
    }
}