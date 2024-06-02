namespace App.Scripts.General.Levels.LevelPackInfoService
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