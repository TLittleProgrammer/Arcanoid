using App.Scripts.General.Levels;
using App.Scripts.General.Levels.LevelPackInfoService;

namespace App.Scripts.Scenes.GameScene.MVVM.Popups.Menu
{
    public sealed class MenuViewModel
    {
        private readonly ILevelPackInfoService _levelPackInfoService;
        
        public MenuViewModel(ILevelPackInfoService levelPackInfoService)
        {
            _levelPackInfoService = levelPackInfoService;
        }

        private LevelPack LevelPack => _levelPackInfoService.LevelPackTransferData.LevelPack;

        public int GetPriceToRestart()
        {
            return LevelPack.EnergyPrice;
        }
    }
}