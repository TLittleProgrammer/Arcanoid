using App.Scripts.General.Command;
using App.Scripts.General.Energy;
using App.Scripts.Scenes.MainMenuScene.Command;
using App.Scripts.Scenes.MainMenuScene.EntryPoint.Initializers;
using App.Scripts.Scenes.MainMenuScene.Features.ActivateScreens;
using App.Scripts.Scenes.MainMenuScene.Features.Buttons;
using App.Scripts.Scenes.MainMenuScene.Features.ContinueLevel;
using App.Scripts.Scenes.MainMenuScene.Features.Factories.ItemView;
using App.Scripts.Scenes.MainMenuScene.Features.Factories.Levels;
using App.Scripts.Scenes.MainMenuScene.Features.LevelPacks;
using App.Scripts.Scenes.MainMenuScene.Features.LevelPacks.MonoBehaviours;
using App.Scripts.Scenes.MainMenuScene.Features.LocaleView;
using App.Scripts.Scenes.MainMenuScene.MVVM.LevelPacks;
using App.Scripts.Scenes.MainMenuScene.MVVM.Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.Scenes.MainMenuScene.EntryPoint
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
        [SerializeField] public SettingsButton SettingsButton;

        public override void InstallBindings()
        {
            Container.Bind<ILevelItemView>().FromInstance(LevelItemView);
            Container.Bind<ILocaleItemView>().FromInstance(LocaleItemView);
            Container.BindInterfacesTo<ContinueLevelService>().AsSingle().WithArguments(ContinueButton).NonLazy();
            
            Container
                .BindInterfacesAndSelfTo<ActivateScreensService>()
                .AsSingle()
                .WithArguments(PlayButton, BackButton, InitialScreen, LevelPacksScreen, ScreenTransitionIamge);

            Container
                .BindFactory<ILevelItemView, ILevelItemView.Factory>()
                .FromFactory<LevelItemFactory>();

            Container
                .BindFactory<LocaleItemView, LocaleItemView.Factory>()
                .FromFactory<LocaleItemViewFactory>();

            Container.BindInterfacesAndSelfTo<SettingsModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<SettingsViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ButtonsHandler>().AsSingle().WithArguments(SettingsButton);

            Container.BindInterfacesAndSelfTo<LevelPackModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelPackViewModel>().AsSingle();
            
            BindCommands();

            Container.BindInterfacesAndSelfTo<MainMenuInitalizer>().AsSingle().WithArguments(EnergyView);
            Container.BindInterfacesAndSelfTo<LevelPacksInitializer>().AsSingle().WithArguments(LevelPackParent);
        }

        private void BindCommands()
        {
            Container
                .BindInterfacesTo<ChangeLocaleCommand>()
                .AsSingle()
                .WhenInjectedInto<SettingsViewModel>();

            Container
                .BindInterfacesTo<LoadLevelCommand>()
                .AsSingle()
                .WhenInjectedInto<LevelPackViewModel>();

            Container
                .BindInterfacesAndSelfTo<ContinueCommand>()
                .AsSingle()
                .WhenInjectedInto<SettingsViewModel>();

            Container.BindInterfacesAndSelfTo<DisableButtonsCommand>().AsSingle();
        }
    }
}