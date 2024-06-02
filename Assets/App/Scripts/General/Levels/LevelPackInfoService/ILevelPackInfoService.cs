using App.Scripts.General.Levels;

namespace App.Scripts.General.LevelPackInfoService
{
    public interface ILevelPackInfoService
    {
        ILevelPackTransferData LevelPackTransferData { get; set; }
        bool NeedContinueLevel { get; }
        ILevelPackTransferData UpdateLevelPackTransferData();
        LevelPack GetDataForNextPack();
        LevelPack GetDataForCurrentPack();
        bool NeedLoadNextPackOrLevel();
        bool NeedLoadNextPack();
    }
}