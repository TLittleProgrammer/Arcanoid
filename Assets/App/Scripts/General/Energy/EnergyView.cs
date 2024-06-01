using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.General.Energy
{
    public class EnergyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _energyText;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private Scrollbar _scrollbar;

        public GameObject Timer => _timerText.gameObject;
        
        public TMP_Text EnergyText => _energyText;
        public TMP_Text TimerText => _timerText;
        public Scrollbar Scrollbar => _scrollbar;
    }
}