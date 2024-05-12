using App.Scripts.External.UserData;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.UserData.Data;
using Zenject;

namespace App.Scripts.General.ProjectInitialization.Installers
{
    public class UserDataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UserDataInstaller>().FromInstance(this).AsSingle();
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            Container.Bind<IUserDataContainer>().To<UserDataContainer>().AsSingle();
            
            Initialize();
        }

        private void Initialize()
        {
            var saveLoad = Container.Resolve<ISaveLoadService>();
            var userDataContainer = Container.Resolve<IUserDataContainer>();
            
            AddSavable<LevelPackProgressDictionary>(saveLoad, userDataContainer);
        }

        private void AddSavable<TSavable>(ISaveLoadService saveLoad, IUserDataContainer userDataContainer) where TSavable : ISavable, new()
        {
            TSavable savable = new TSavable();
            saveLoad.Load(ref savable);
            userDataContainer.AddData(savable);
        }
    }
}