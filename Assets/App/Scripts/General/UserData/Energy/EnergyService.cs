using System;
using App.Scripts.External.UserData;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.UserData.Energy
{
    public class EnergyDataService : IEnergyDataService
    {
        private readonly IUserDataContainer _userDataContainer;
        
        private EnergyData _energyData;
        public event Action<int> ValueChanged;

        public EnergyDataService(IUserDataContainer userDataContainer)
        {
            _userDataContainer = userDataContainer;
        }

        public int CurrentValue => _energyData.Value;

        public async UniTask AsyncInitialize()
        {
            _energyData = (EnergyData)_userDataContainer.GetData<EnergyData>();

            await UniTask.CompletedTask;
        }

        public void Add(int value)
        {
            if (_energyData.Value + value < 0)
            {
                _energyData.Value = 0;
            }
            else
            {
                _energyData.Value += value;
            }
            
            ValueChanged?.Invoke(_energyData.Value);
            _userDataContainer.SaveData<EnergyData>();
        }
    }
}