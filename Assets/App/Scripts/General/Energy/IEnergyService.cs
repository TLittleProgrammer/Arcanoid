using System;
using App.Scripts.External.Initialization;

namespace App.Scripts.General.Energy
{
    public interface IEnergyService : IAsyncInitializable, IDisposable
    {
        void AddView(EnergyView view);
        void RemoveView(EnergyView view);
        void SetSecondsToAddEnergy(int seconds);
    }
}