﻿using System;
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
        }

        public TClassSavable GetData()
        {
            if (_savable is null)
            {
                _saveLoadService.Load(ref _savable, _fileName);
            }

            return _savable;
        }

        public void SaveData()
        {
            _saveLoadService.Save(_savable, _fileName);
        }

        public void Delete()
        {
            _saveLoadService.Delete(_fileName);
        }
    }
}