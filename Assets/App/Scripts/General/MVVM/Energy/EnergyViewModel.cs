using System;
using System.Collections.Generic;
using App.Scripts.General.Energy;
using App.Scripts.General.UserData.Energy;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.MVVM.Energy
{
    public class EnergyViewModel : IInitializable, IDisposable
    {
        private readonly EnergyModel _energyModel;
        private readonly IEnergyDataService _energyDataService;

        private const string _energyValueFormat = "{0}/{1}";
        
        private List<EnergyView> _views;

        public EnergyViewModel(EnergyModel energyModel)
        {
            _energyModel = energyModel;
        }

        public void Initialize()
        {
            _energyModel.RemainingSeconds += OnRemaningSeconds;
            _energyModel.ValueChanged += OnValueChanged;
            
            _views = new();
        }

        public void AddView(EnergyView energyView)
        {
            _views.Add(energyView);

            InitializeView(energyView);
        }

        public void RemoveView(EnergyView energyView)
        {
            _views.Remove(energyView);
        }

        private void InitializeView(EnergyView energyView)
        {
            energyView.EnergyText.text = string.Format(_energyValueFormat, _energyModel.CurrentEnergy.ToString(), _energyModel.MaxEnergy.ToString());
            energyView.Scrollbar.size = (float)_energyModel.CurrentEnergy / _energyModel.MaxEnergy;

            if (_energyModel.CurrentEnergy < _energyModel.MaxEnergy)
            {
                int minutes = _energyModel.SecondsRemaining / 60;
                int seconds = _energyModel.SecondsRemaining % 60;

                energyView.TimerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
                energyView.Timer.SetActive(true);
            }
            else
            {
                energyView.Timer.SetActive(false);
            }
        }

        private void OnRemaningSeconds(int remainingSeconds)
        {
            int minutes = remainingSeconds / 60;
            int seconds = remainingSeconds % 60;

            UpdateTimer(minutes, seconds);
        }

        private void OnValueChanged(int newValue, int max)
        {
            float scrollValue = (float)newValue / max;
            
            foreach (EnergyView view in _views)
            {
                view.EnergyText.text = string.Format(_energyValueFormat, newValue.ToString(), max.ToString());

                view.Scrollbar.size = scrollValue;
            }
        }

        private void UpdateTimer(int minutes, int seconds)
        {
            foreach (EnergyView view in _views)
            {
                view.TimerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
            }
        }

        public void Dispose()
        {
            _views.Clear();
        }
    }
}