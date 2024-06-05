using System;
using App.Scripts.General.UserData.Levels;
using UnityEngine;

namespace App.Scripts.General.Levels.LevelPackInfoService
{
    public sealed class LevelPackInfoService : ILevelPackInfoService
    {
        private readonly LevelPackProvider _levelPackProvider;
        private readonly LevelPackProgressDataService _levelPackProgressDataService;
        
        private ILevelPackTransferData _levelPackTransferData;

        public LevelPackInfoService(
            LevelPackProvider levelPackProvider,
            LevelPackProgressDataService levelPackProgressDataService)
        {
            _levelPackProvider = levelPackProvider;
            _levelPackProgressDataService = levelPackProgressDataService;
        }

        public bool NeedContinueLevel => _levelPackTransferData?.NeedContinue ?? false;

        public ILevelPackTransferData LevelPackTransferData
        {
            get => _levelPackTransferData;
            set => _levelPackTransferData = value;
        }

        public ILevelPackTransferData UpdateLevelPackTransferData()
        {
            ILevelPackTransferData data = new LevelPackTransferData();

            _levelPackProgressDataService.PassLevel(_levelPackTransferData.PackIndex, _levelPackTransferData.LevelIndex);
            
            UpdatePackIndexAndLevelIndex(_levelPackTransferData, data);
            LoadLevelPack(data);
            
            data.NeedLoadLevel = true;

            _levelPackTransferData = data;

            return data;
        }

        public LevelPack GetDataForNextPack()
        {
            if (_levelPackProvider.LevelPacks.Count <= _levelPackTransferData.PackIndex + 1)
            {
                return null;
            }
            return _levelPackProvider.LevelPacks[_levelPackTransferData.PackIndex + 1];
        }

        public LevelPack GetDataForCurrentPack()
        {
            try
            {
                return _levelPackProvider.LevelPacks[_levelPackTransferData.PackIndex];
            }
            catch (NullReferenceException nullReference)
            {
                Debug.Log($"Встречен nullref при попытке получить текущий пак, уровень запускался сразу с игровой сцены\n{nullReference}");
                return null;
            }
        }

        public bool NeedLoadNextPackOrLevel()
        {
            if (_levelPackProgressDataService.GetPassedLevelsForPackIndex(_levelPackTransferData.PackIndex) == 0 ||
                _levelPackTransferData.LevelIndex + 1 < _levelPackTransferData.LevelPack.Levels.Count)
            {
                return true;
            }
            
            if (_levelPackProvider.LevelPacks.Count <= _levelPackTransferData.PackIndex + 1)
            {
                _levelPackProgressDataService.PassLevel(_levelPackTransferData.PackIndex, _levelPackTransferData.LevelIndex);
                return false;
            }

            return true;
        }

        public bool NeedLoadNextPack()
        {
            return _levelPackProgressDataService.GetPassedLevelsForPackIndex(_levelPackTransferData.PackIndex + 1) == 0 &&
                   _levelPackTransferData.LevelIndex + 1 >= _levelPackTransferData.LevelPack.Levels.Count;
        }

        private void LoadLevelPack(ILevelPackTransferData data)
        {
            data.LevelPack = _levelPackProvider.LevelPacks[data.PackIndex];
        }

        private void UpdatePackIndexAndLevelIndex(ILevelPackTransferData levelPackTransferData, ILevelPackTransferData data)
        {
            levelPackTransferData.LevelIndex++;
            
            if (levelPackTransferData.LevelIndex >= levelPackTransferData.LevelPack.Levels.Count)
            {
                if (_levelPackProvider.LevelPacks.Count < levelPackTransferData.PackIndex + 1)
                {
                    data.PackIndex = 0;
                }
                else
                {
                    data.PackIndex = levelPackTransferData.PackIndex + 1;
                }
                
                data.LevelIndex = 0;
            }
            else
            {
                data.LevelIndex = levelPackTransferData.LevelIndex;
                data.PackIndex = levelPackTransferData.PackIndex;
            }
        }
    }
}