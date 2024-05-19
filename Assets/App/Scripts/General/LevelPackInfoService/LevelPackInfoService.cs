using App.Scripts.General.Levels;
using App.Scripts.General.UserData.Services;

namespace App.Scripts.General.LevelPackInfoService
{
    public sealed class LevelPackInfoService : ILevelPackInfoService
    {
        private readonly LevelPackProvider _levelPackProvider;
        private readonly LevelProgressDataService _levelProgressDataService;
        
        private ILevelPackTransferData _levelPackTransferData;

        public LevelPackInfoService(
            LevelPackProvider levelPackProvider,
            LevelProgressDataService levelProgressDataService)
        {
            _levelPackProvider = levelPackProvider;
            _levelProgressDataService = levelProgressDataService;
        }

        public ILevelPackTransferData UpdateLevelPackTransferData()
        {
            ILevelPackTransferData data = new LevelPackTransferData();

            _levelProgressDataService.PassLevel(_levelPackTransferData.PackIndex, _levelPackTransferData.LevelIndex);
            
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

            return _levelProgressDataService.GetPassedLevelsForPackIndex(_levelPackTransferData.PackIndex + 1) == 0 ||
                   _levelPackTransferData.LevelIndex + 1 < _levelPackTransferData.LevelPack.Levels.Count;;
        }

        public bool NeedLoadNextPack()
        {
            return _levelProgressDataService.GetPassedLevelsForPackIndex(_levelPackTransferData.PackIndex + 1) == 0 &&
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