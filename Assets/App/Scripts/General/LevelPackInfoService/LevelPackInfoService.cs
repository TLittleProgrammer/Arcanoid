using App.Scripts.General.Levels;
using App.Scripts.General.UserData.Levels;

namespace App.Scripts.General.LevelPackInfoService
{
    public sealed class LevelPackInfoService : ILevelPackInfoService
    {
        private readonly LevelPackProvider _levelPackProvider;
        private readonly LevelProgressDataService _levelProgressDataService;

        public LevelPackInfoService(LevelPackProvider levelPackProvider, LevelProgressDataService levelProgressDataService)
        {
            _levelPackProvider = levelPackProvider;
            _levelProgressDataService = levelProgressDataService;
        }

        public ILevelPackTransferData UpdateLevelPackTransferData(ILevelPackTransferData levelPackTransferData)
        {
            ILevelPackTransferData data = new LevelPackTransferData();

            _levelProgressDataService.PassLevel(levelPackTransferData.PackIndex);
            
            UpdatePackIndexAndLevelIndex(levelPackTransferData, data);
            LoadLevelPack(data);
            
            data.NeedLoadLevel = true;

            return data;
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
                if (_levelPackProvider.LevelPacks.Count <= levelPackTransferData.PackIndex + 1)
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
            }
        }
    }
}