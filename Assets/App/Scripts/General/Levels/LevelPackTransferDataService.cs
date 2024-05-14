namespace App.Scripts.General.Levels
{
    public sealed class LevelPackTransferDataService : ILevelPackTransferData
    {
        public bool NeedLoadLevel { get; set; }
        public int LevelIndex { get; set; }
        public int PackIndex { get; set; }
        public float LevelPackProgress { get; set; }
        public LevelPack LevelPack { get; set; }
    }
}