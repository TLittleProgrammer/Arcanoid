using App.Scripts.General.Components;
using App.Scripts.General.UserData;
using App.Scripts.General.UserData.Data;
using App.Scripts.Scenes.MainMenuScene.Factories.Levels;
using App.Scripts.Scenes.MainMenuScene.LevelPacks;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.Configs;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours;
using Sirenix.Serialization;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [OdinSerialize] public LevelItemView LevelItemView;
        [OdinSerialize] public LevelPackParent LevelPackParent;
        
        public override void InstallBindings()
        {
            LevelPackProgressDictionary levelPackProgressDictionary =
                (LevelPackProgressDictionary)Container.Resolve<IUserDataContainer>().GetData<LevelPackProgressDictionary>();

            Container
                .Bind<LevelItemFactory>()
                .AsSingle()
                .WithArguments(LevelItemView, LevelPackParent,levelPackProgressDictionary);
        }
    }
}