using App.Scripts.External.UserData;
using App.Scripts.General.Components;
using App.Scripts.General.Levels;
using App.Scripts.General.UserData.Data;
using App.Scripts.Scenes.MainMenuScene.Factories.Levels;
using App.Scripts.Scenes.MainMenuScene.LevelPacks;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours;
using Sirenix.Serialization;
using UnityEngine.UIElements;
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

            Container.Bind<ILevelItemView>().FromInstance(LevelItemView);
            Container.Bind<ITransformable>().FromInstance(LevelPackParent);
            Container.Bind<LevelPackProgressDictionary>().FromInstance(levelPackProgressDictionary);
            
            Container
                .BindFactory<int, LevelPack, ILevelItemView, ILevelItemView.Factory>()
                .FromFactory<LevelItemFactory>();
        }
    }
}