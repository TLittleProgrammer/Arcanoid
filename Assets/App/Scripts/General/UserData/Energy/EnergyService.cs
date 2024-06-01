using System;
using App.Scripts.External.UserData;
using App.Scripts.General.LevelPackInfoService;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.UserData.Energy
{
    public class EnergyDataService : IEnergyDataService, IInitializable
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

        public void Initialize()
        {
            _energyData = _energyDataProvider.GetData();
        }

        public void Add(int value)
        {
            Debug.Log(value);
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