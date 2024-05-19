using App.Scripts.General.Levels;

namespace App.Scripts.General.LevelPackInfoService
{
    public interface ILevelPackInfoService
    {
        ILevelPackTransferData UpdateLevelPackTransferData();
        ILevelPackTransferData GetData();
        void SetData(ILevelPackTransferData levelPackTransferData);
        bool NeedLoadNextPackOrLevel();
    }
}