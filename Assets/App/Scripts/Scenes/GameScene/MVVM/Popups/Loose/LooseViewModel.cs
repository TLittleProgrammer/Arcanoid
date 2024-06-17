using App.Scripts.General.Levels;
using App.Scripts.General.Levels.LevelPackInfoService;

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
        private int EnergyPrice => LevelPack.EnergyPrice;

        public int GetPriceToRestart()
        {
            return EnergyPrice;
        }

        public int GetPriceToContinue()
        {
            return EnergyPrice * 2;
        }

        public bool CanRestart(int energyValue)
        {
            return EnergyPrice <= energyValue;
        }

        public bool CanContinue(int energyValue)
        {
            return EnergyPrice * 2 <= energyValue;
        }
    }
}