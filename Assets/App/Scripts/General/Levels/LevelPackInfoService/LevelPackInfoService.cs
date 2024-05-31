using System;
using App.Scripts.General.Levels;
using App.Scripts.General.UserData.Levels;

namespace App.Scripts.General.LevelPackInfoService
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

        public bool NeedContinueLevel => _levelPackTransferData.NeedContinue;

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

        public ILevelPackTransferData GetData()
        {
            return _levelPackTransferData;
        }

        public LevelPack GetDataForNextPack()
        {
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
                return null;
            }
        }

        public void SetData(ILevelPackTransferData levelPackTransferData)
        {
            _levelPackTransferData = levelPackTransferData;
        }

        public bool NeedLoadNextPackOrLevel()
        {
            if (_levelPackProvider.LevelPacks.Count < _levelPackTransferData.PackIndex + 1)
            {
                return false;
            }

            return _levelPackProgressDataService.GetPassedLevelsForPackIndex(_levelPackTransferData.PackIndex + 1) == 0 ||
                   _levelPackTransferData.LevelIndex + 1 < _levelPackTransferData.LevelPack.Levels.Count;;
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