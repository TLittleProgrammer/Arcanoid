﻿using System.Collections.Generic;
using App.Scripts.External.DotweenContainerService;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.Localisation;
using App.Scripts.External.Localisation.AssetManagment;
using App.Scripts.External.Localisation.Converters;
using App.Scripts.External.SceneManagment;
using App.Scripts.General.DateTime;
using App.Scripts.General.Energy;
using App.Scripts.General.Energy.Handlers;
using App.Scripts.General.Game;
using App.Scripts.General.Levels.LevelPackInfoService;
using App.Scripts.General.MVVM.Energy;
using App.Scripts.General.Popup;
using App.Scripts.General.Popup.Factory;
using App.Scripts.General.States;
using App.Scripts.General.Time;
using App.Scripts.General.UserData.Energy;
using App.Scripts.General.UserData.Global;
using App.Scripts.General.UserData.Levels;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using EnergyViewModel = App.Scripts.General.MVVM.Energy.EnergyViewModel;

namespace App.Scripts.General.ProjectInitialization.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [FolderPath]
        public string PathToLocaleResources;
        
        [SerializeField] private GameStatements _gameStatements;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TimeTicker>().AsSingle();

            Container.BindInterfacesAndSelfTo<IGameStatements>().FromInstance(_gameStatements).AsSingle();
            
            Container.Bind<ISceneManagementService>().To<SceneManagementService>().AsSingle();
            Container.Bind<IDotweenContainerService>().To<DotweenContainerService>().AsSingle();
            Container.Bind<LevelPackProgressDataService>().AsSingle();
            Container.Bind<ILocaleService>().To<LocaleService>().AsSingle();
            Container.Bind<IConverter>().To<CsvConverter>().AsSingle();
            Container.Bind<IDateTimeService>().To<DateTimeService>().AsSingle();
            Container.Bind<IGlobalDataService>().To<GlobalDataService>().AsSingle();
            Container.Bind<InfoBetweenScenes.InfoBetweenScenes>().AsSingle();
            Container.Bind<IPopupFactory>().To<PopupFactory>().AsSingle();
            Container.Bind<IPopupService>().To<PopupService>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnergyInitializer>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnergySaveHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnergyDataService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingSceneState>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnergyModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<EnergyViewModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ResourcesLocaleAssetProvider>().AsSingle().WithArguments(PathToLocaleResources);
            Container.BindInterfacesAndSelfTo<LocaleMappingFromTextAsset>().AsSingle();
            
            Container.Bind<ILevelPackInfoService>().To<LevelPackInfoService>().AsSingle();
            BindStatemachine();
            
            Container.BindInterfacesAndSelfTo<ProjectInitializer>().AsSingle();
        }

        private void BindStatemachine()
        {
            Container.Bind<IStateMachine>().FromMethod(ctx =>
            {
                var services = ctx.Container.Resolve<List<IExitableState>>();

                IStateMachine stateMachine = new StateMachine();

                stateMachine.AsyncInitialize(services);
                return stateMachine;
            }).AsSingle();
        }
    }
}