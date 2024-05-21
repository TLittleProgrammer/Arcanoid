using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.External.UserData;
using App.Scripts.General.Levels;
using App.Scripts.General.Popup;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.General.Popup.Factory;
using App.Scripts.General.UserData.Levels.Data;
using App.Scripts.Scenes.MainMenuScene.Factories.ItemView;
using App.Scripts.Scenes.MainMenuScene.Factories.Levels;
using App.Scripts.Scenes.MainMenuScene.LevelPacks;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours;
using App.Scripts.Scenes.MainMenuScene.LocaleView;
using Sirenix.Serialization;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [OdinSerialize] public LevelItemView LevelItemView;
        [OdinSerialize] public LevelPackParent LevelPackParent;
        [OdinSerialize] public LocaleItemView LocaleItemView;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainMenuInitalizer>().AsSingle();
            
            LevelPackProgressDictionary levelPackProgressDictionary =
                (LevelPackProgressDictionary)Container.Resolve<IUserDataContainer>().GetData<LevelPackProgressDictionary>();

            Container.Bind<ILevelItemView>().FromInstance(LevelItemView);
            Container.Bind<ILocaleItemView>().FromInstance(LocaleItemView);
            Container.Bind<ITransformable>().FromInstance(LevelPackParent);
            Container.Bind<LevelPackProgressDictionary>().FromInstance(levelPackProgressDictionary);
            
            Container.Bind<IPopupProvider>().To<ResourcesPopupProvider>().AsSingle();
            Container.Bind<IPopupFactory>().To<PopupFactory>().AsSingle();
            Container.Bind<IPopupService>().To<PopupService>().AsSingle();
            
            
            Container
                .BindFactory<int, LevelPack, ILevelItemView, ILevelItemView.Factory>()
                .FromFactory<LevelItemFactory>();

            Container
                .BindFactory<List<LocaleItemView>, LocaleItemView.Factory>()
                .FromFactory<LocaleItemViewFactory>();
        }
    }
}