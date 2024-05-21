using App.Scripts.External.Initialization;

namespace App.Scripts.General.Energy
{
    public interface IEnergyService : IAsyncInitializable
    {
        void AddView(EnergyView view);
        void RemoveView(EnergyView view);
    }
}