using System;
using App.Scripts.External.UserData;
using App.Scripts.General.LevelPackInfoService;
using Cysharp.Threading.Tasks;

namespace App.Scripts.General.UserData.Energy
{
    public class EnergyDataService : IEnergyDataService
    {
        private readonly IDataProvider<EnergyData> _energyDataProvider;
        private readonly ILevelPackInfoService _levelPackInfoService;

        private EnergyData _energyData;
        public event Action<int> ValueChanged;

        public EnergyDataService(IDataProvider<EnergyData> energyDataProvider, ILevelPackInfoService levelPackInfoService)
        {
            _energyDataProvider = energyDataProvider;
            _levelPackInfoService = levelPackInfoService;
        }

        public int CurrentValue => _energyData.Value;

        public async UniTask AsyncInitialize()
        {
            _energyData = _energyDataProvider.GetData();

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
            _energyDataProvider.SaveData(_energyData);
        }

        public void AddEnergyByPassedLevel()
        {
            Add(_levelPackInfoService.GetDataForCurrentPack().EnergyAddForWin);
        }
    }
}