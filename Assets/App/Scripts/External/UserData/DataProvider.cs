using System;
using App.Scripts.External.UserData.SaveLoad;

namespace App.Scripts.External.UserData
{
    public sealed class DataProvider<TClassSavable> : IDataProvider<TClassSavable> where TClassSavable : class, ISavable
    {
        private readonly ISaveLoadService _saveLoadService;
        private readonly string _fileName;
        
        private TClassSavable _savable;

        public DataProvider(ISaveLoadService saveLoadService, string fileName)
        {
            _saveLoadService = saveLoadService;
            _fileName = fileName;
            _savable = (TClassSavable)Activator.CreateInstance(typeof(TClassSavable));
        }

        public TClassSavable GetData()
        {
            _saveLoadService.Load(ref _savable, _fileName);

            return _savable;
        }

        public void SaveData(TClassSavable savable)
        {
            _saveLoadService.Save(savable, _fileName);
        }

        public void Delete()
        {
            _saveLoadService.Delete(_fileName);
        }
    }
}