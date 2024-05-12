using App.Scripts.General.UserData.SaveLoad;

namespace App.Scripts.General.UserData
{
    public interface IUserDataContainer
    {
        void AddData<TSavable>(TSavable savable) where TSavable : ISavable;
        ISavable GetData<TSavable>() where TSavable : ISavable;
    }
}