namespace App.Scripts.General.Levels
{
    public interface ILevelPackTransferData
    {
        bool NeedLoadLevel { get; set; }
        int LevelIndex { get; set; }
        float LevelPackProgress { get; set; }
        LevelPack LevelPack { get; set; }
    }
}