﻿using App.Scripts.External.UserData.SaveLoad;

namespace App.Scripts.External.UserData
{
    public interface IUserDataContainer
    {
        void AddData<TSavable>(TSavable savable) where TSavable : ISavable;
        ISavable GetData<TSavable>() where TSavable : ISavable;
    }
}