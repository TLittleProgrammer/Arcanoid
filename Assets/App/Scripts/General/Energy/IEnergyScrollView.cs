using TMPro;
using UnityEngine.UI;

namespace App.Scripts.General.Energy
{
    public interface IEnergyScrollView
    {
        TMP_Text CurrentEnergy { get; }
        TMP_Text TimeToGetEnergy { get; }
        Scrollbar Scrollbar { get; }

        void SetEnergyText(int current);
        void SetTimeToGetEnergy(int minutes, int seconds);
    }
}