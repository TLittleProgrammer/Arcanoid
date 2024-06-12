namespace App.Scripts.External.UserData.SaveLoad
{
    public interface ISaveLoadService
    {
        void Save(ISavable savable, string fileName);
        void Load<TSavable>(ref TSavable savable, string fileName) where TSavable : ISavable;
        bool Exists(string currentlevelprogressJson);
        void Delete(string fileName);
    }
}