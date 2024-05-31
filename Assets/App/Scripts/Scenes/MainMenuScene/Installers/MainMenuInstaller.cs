using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.External.UserData;
using App.Scripts.General.Energy;
using App.Scripts.General.Levels;
using App.Scripts.General.Popup;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.General.Popup.Factory;
using App.Scripts.General.RootUI;
using App.Scripts.General.UserData.Levels.Data;
using App.Scripts.Scenes.MainMenuScene.ActivateScreens;
using App.Scripts.Scenes.MainMenuScene.ContinueLevel;
using App.Scripts.Scenes.MainMenuScene.Factories.ItemView;
using App.Scripts.Scenes.MainMenuScene.Factories.Levels;
using App.Scripts.Scenes.MainMenuScene.LevelPacks;
using App.Scripts.Scenes.MainMenuScene.LevelPacks.MonoBehaviours;
using App.Scripts.Scenes.MainMenuScene.LocaleView;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] public LevelItemView LevelItemView;
        [SerializeField] public LevelPackParent LevelPackParent;
        [SerializeField] public LocaleItemView LocaleItemView;
        [SerializeField] public EnergyView EnergyView;
        [SerializeField] public Button PlayButton;
        [SerializeField] public Button BackButton;
        [SerializeField] public GameObject InitialScreen;
        [SerializeField] public GameObject LevelPacksScreen;
        [SerializeField] public Image ScreenTransitionIamge;
        [SerializeField] public Button ContinueButton;

        [Inject] private RootUIViewProvider _rootUIView;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainMenuInitalizer>().AsSingle().WithArguments(EnergyView);
            
            LevelPackProgressDictionary levelPackProgressDictionary =
                (LevelPackProgressDictionary)Container.Resolve<IUserDataContainer>().GetData<LevelPackProgressDictionary>();

            Container.Bind<ILevelItemView>().FromInstance(LevelItemView);
            Container.Bind<ILocaleItemView>().FromInstance(LocaleItemView);
            Container.Bind<ITransformable>().FromInstance(LevelPackParent);
            Container.Bind<LevelPackProgressDictionary>().FromInstance(levelPackProgressDictionary);
            Container.BindInterfacesTo<ContinueLevelService>().AsSingle().WithArguments(ContinueButton).NonLazy();
            
            Container.Bind<IPopupProvider>().To<ResourcesPopupProvider>().AsSingle();
            Container.Bind<IPopupFactory>().To<PopupFactory>().AsSingle();
            Container.Bind<IPopupService>().To<PopupService>().AsSingle().WithArguments(_rootUIView.BackPopupPlane);

            Container
                .BindInterfacesAndSelfTo<ActivateScreensService>()
                .AsSingle()
                .WithArguments(PlayButton, BackButton, InitialScreen, LevelPacksScreen, ScreenTransitionIamge);
            
            
            
            Container
                .BindFactory<int, LevelPack, ILevelItemView, ILevelItemView.Factory>()
                .FromFactory<LevelItemFactory>();

            Container
                .BindFactory<List<LocaleItemView>, LocaleItemView.Factory>()
                .FromFactory<LocaleItemViewFactory>();
        }
    }
}