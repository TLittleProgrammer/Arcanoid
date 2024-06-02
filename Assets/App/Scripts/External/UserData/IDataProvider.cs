using App.Scripts.External.UserData.SaveLoad;

namespace App.Scripts.External.UserData
{
    public interface IDataProvider<TClassSavable> where TClassSavable : class, ISavable
    {
        TClassSavable GetData();
        void SaveData(TClassSavable savable);
        void Delete();
    }
}