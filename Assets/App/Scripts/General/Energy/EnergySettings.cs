using UnityEngine;

namespace App.Scripts.General.Energy
{
    [CreateAssetMenu(menuName = "Configs/Settings/EnergySettings", fileName = "EnergySettings")]
    public class EnergySettings : ScriptableObject
    {
        public int MaxEnergyCount;
        public int InitialEnergyCount;
        public int SecondsToRecoveryEnergy;
    }
}