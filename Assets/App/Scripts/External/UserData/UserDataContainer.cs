using System;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.UserData.SaveLoad;

namespace App.Scripts.External.UserData
{
    public class UserDataContainer : IUserDataContainer
    {
        private readonly Dictionary<Type,ISavable> _savables = new();

        public UserDataContainer()
        {
            _savables = new();
        }
        
        public UserDataContainer(IEnumerable<ISavable> savables)
        {
            _savables = savables
                .ToDictionary(x => x.GetType(), x => x);
        }

        public void AddData<TSavable>(TSavable savable) where TSavable : ISavable
        {
            _savables.Add(savable.GetType(), savable);
        }

        public ISavable GetData<TSavable>() where TSavable : ISavable
        {
            return _savables.TryGetValue(typeof(TSavable), out ISavable savable) ? savable : null;
        }
    }
}