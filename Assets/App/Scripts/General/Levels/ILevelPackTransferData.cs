namespace App.Scripts.General.Levels
{
    public interface ILevelPackTransferData
    {
        bool NeedLoadLevel { get; set; }
        public bool NeedContinue { get; set; }
        int PackIndex { get; set; }
        int LevelIndex { get; set; }
        LevelPack LevelPack { get; set; }
    }
}