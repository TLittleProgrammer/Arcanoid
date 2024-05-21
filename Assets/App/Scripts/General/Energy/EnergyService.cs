using System.Collections.Generic;
using App.Scripts.General.Time;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.Energy
{
    public class EnergyService : IEnergyService
    {
        private readonly ITimeTicker _ticker;
        private readonly EnergySettings _energySettings;

        private List<EnergyView> _views = new();
        private int _secondsToAddEnergy;
        
        public EnergyService(ITimeTicker ticker, EnergySettings energySettings)
        {
            _ticker = ticker;
            _energySettings = energySettings;
            
            
        }

        public async UniTask AsyncInitialize()
        {
            _ticker.SecondsTicked += OnSecondsTicked;
            _secondsToAddEnergy = _energySettings.SecondsToRecoveryEnergy;
            
            await UniTask.CompletedTask;
        }

        private void OnSecondsTicked()
        {
            
        }
    }
}