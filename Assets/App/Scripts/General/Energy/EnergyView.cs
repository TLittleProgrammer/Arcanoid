using App.Scripts.General.MVVM.Energy;
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
        private EnergyViewModel _viewModel;

        public GameObject Timer => _timerText.gameObject;
        
        public TMP_Text EnergyText => _energyText;
        public TMP_Text TimerText => _timerText;
        public Scrollbar Scrollbar => _scrollbar;

        private const string _energyValueFormat = "{0}/{1}";
        
        public void Initialize(EnergyViewModel viewModel)
        {
            _viewModel = viewModel;
            
            EnergyText.text = string.Format(_energyValueFormat, viewModel.CurrentEnergy.Value.ToString(), viewModel.MaxEnergy.ToString());
            Scrollbar.size = (float)viewModel.CurrentEnergy.Value / viewModel.MaxEnergy;

            ShowOrHideTimer(viewModel);
            
            viewModel.SecondsToAddEnergy.OnChanged += OnViewModelRemainingSecondsChanged;
            viewModel.CurrentEnergy.OnChanged += OnViewModelCurrentEnergyChanged;
            
            OnViewModelRemainingSecondsChanged(viewModel.SecondsToAddEnergy.Value);
        }

        private void OnViewModelCurrentEnergyChanged(int energy)
        {
            float scrollValue = (float)energy / _viewModel.MaxEnergy;
            
            EnergyText.text = string.Format(_energyValueFormat, energy.ToString(), _viewModel.MaxEnergy.ToString());
            Scrollbar.size = scrollValue;
        }

        private void ShowOrHideTimer(EnergyViewModel viewModel)
        {
            Timer.SetActive(viewModel.CurrentEnergy.Value < viewModel.MaxEnergy);
        }

        private void OnViewModelRemainingSecondsChanged(int remainingSeconds)
        {
            int minutes = remainingSeconds / 60;
            int seconds = remainingSeconds % 60;
            
            TimerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
        }
    }
}