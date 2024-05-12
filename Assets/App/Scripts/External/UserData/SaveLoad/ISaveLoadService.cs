namespace App.Scripts.External.UserData.SaveLoad
{
    public interface ISaveLoadService
    {
        void Save(ISavable savable);
        void Load<TSavable>(ref TSavable savable) where TSavable : ISavable;
    }
}