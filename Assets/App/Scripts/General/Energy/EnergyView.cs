using System;
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

        private const string _energyValueFormat = "{0}/{1}";
        private const string _energyTimerFormat = "{0}:{1}";

        public void UpdateTimer(int minutes, int seconds)
        {
            string secondsStr = seconds >= 10 ? seconds.ToString() : $"0{seconds.ToString()}";
            
            _timerText.text = String.Format(_energyTimerFormat, minutes.ToString(), secondsStr);
        }

        public void UpdateEnergyValue(int current, int max, float scrollValue)
        {
            _energyText.text = String.Format(_energyValueFormat, current.ToString(), max.ToString());

            _scrollbar.size = scrollValue;
        }
    }
}