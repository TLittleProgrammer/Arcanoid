using UnityEngine;

namespace App.Scripts.General.ProjectInitialization.Settings
{
    [CreateAssetMenu(menuName = "Configs/Settings/Energy Settings", fileName = "EnergySettings")]
    public class EnergySettings : ScriptableObject
    {
        public int MaxEnergyCounter;
        public int SecondsToAddOneEnergy;
    }
}