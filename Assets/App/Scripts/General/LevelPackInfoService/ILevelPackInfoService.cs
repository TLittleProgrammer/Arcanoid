using App.Scripts.General.Levels;

namespace App.Scripts.General.LevelPackInfoService
{
    public interface ILevelPackInfoService
    {
        ILevelPackTransferData UpdateLevelPackTransferData();
        ILevelPackTransferData GetData();
        LevelPack GetDataForNextPack();
        void SetData(ILevelPackTransferData levelPackTransferData);
        bool NeedLoadNextPackOrLevel();
        bool NeedLoadNextPack();
    }
}