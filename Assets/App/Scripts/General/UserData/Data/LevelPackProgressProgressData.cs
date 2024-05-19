namespace App.Scripts.General.UserData.Data
{
    public sealed class LevelPackProgressProgressData
    {
        public int PassedLevels;

        public LevelPackProgressProgressData()
        {
            PassedLevels = 0;
        }

        public LevelPackProgressProgressData(int passedLevels)
        {
            PassedLevels = passedLevels;
        }
    }
}