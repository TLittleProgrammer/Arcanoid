using App.Scripts.External.UserData;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.UserData.Constants;
using App.Scripts.General.UserData.Energy;
using App.Scripts.General.UserData.Global;
using App.Scripts.General.UserData.Levels.Data;
using Zenject;

namespace App.Scripts.General.ProjectInitialization.Installers
{
    public class UserDataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            
            Container.Bind<IDataProvider<GlobalData>>().To<DataProvider<GlobalData>>().AsSingle().WithArguments(SavableConstants.GlobalFileName);
            Container.Bind<IDataProvider<EnergyData>>().To<DataProvider<EnergyData>>().AsSingle().WithArguments(SavableConstants.EnergyFileName);
            Container.Bind<IDataProvider<LevelPackProgressDictionary>>().To<DataProvider<LevelPackProgressDictionary>>().AsSingle().WithArguments(SavableConstants.LevelProgressFileName);
        }
    }
}