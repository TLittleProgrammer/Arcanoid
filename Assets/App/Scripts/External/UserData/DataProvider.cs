using System;
using App.Scripts.External.UserData.SaveLoad;

namespace App.Scripts.External.UserData
{
    public sealed class DataProvider<TClassSavable> : IDataProvider<TClassSavable> where TClassSavable : class, ISavable
    {
        private readonly ISaveLoadService _saveLoadService;

        public DataProvider(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public TClassSavable GetData()
        {
            TClassSavable classSavable = (TClassSavable)Activator.CreateInstance(typeof(TClassSavable));
            
            _saveLoadService.Load(ref classSavable);

            return classSavable;
        }

        public void SaveData(TClassSavable savable)
        {
            _saveLoadService.Save(savable);
        }
    }
}