using App.Scripts.General.Levels;
using App.Scripts.General.Levels.LevelPackInfoService;
using App.Scripts.Scenes.GameScene.Features.Popups;

namespace App.Scripts.Scenes.GameScene.MVVM.Popups.Loose
{
    public class LooseViewModel
    {
        private readonly ILevelPackInfoService _levelPackInfoService;
        
        public LooseViewModel(ILevelPackInfoService levelPackInfoService)
        {
            _levelPackInfoService = levelPackInfoService;
        }

        private LevelPack LevelPack => _levelPackInfoService.LevelPackTransferData.LevelPack;

        public int GetPriceToRestart()
        {
            return LevelPack.EnergyPrice * 2;
        }

        public bool CanRestart(int energyValue)
        {
            return LevelPack.EnergyPrice * 2 <= energyValue;
        }
    }
}