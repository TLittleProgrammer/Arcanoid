using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.External.DotweenContainerService;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.Localisation;
using App.Scripts.External.Localisation.Converters;
using App.Scripts.External.SceneManagment;
using App.Scripts.General.DateTime;
using App.Scripts.General.Energy;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.Popup;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.General.Popup.Factory;
using App.Scripts.General.RootUI;
using App.Scripts.General.States;
using App.Scripts.General.Time;
using App.Scripts.General.UserData.Energy;
using App.Scripts.General.UserData.Global;
using App.Scripts.General.UserData.Levels;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.ProjectInitialization.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public RootUIViewProvider RootUIPrefab;
        public GameObject LoadingScreenPrefab;
        public TextAsset Localisation;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TimeTicker>().AsSingle();
            
            Container.Bind<ISceneManagementService>().To<SceneManagementService>().AsSingle();
            Container.Bind<IDotweenContainerService>().To<DotweenContainerService>().AsSingle();
            Container.Bind<LevelPackProgressDataService>().AsSingle();
            Container.Bind<ILocaleService>().To<LocaleService>().AsSingle();
            Container.Bind<IConverter>().To<CsvConverter>().AsSingle();
            Container.Bind<IEnergyDataService>().To<EnergyDataService>().AsSingle();
            Container.Bind<IEnergyService>().To<EnergyService>().AsSingle();
            Container.Bind<IDateTimeService>().To<DateTimeService>().AsSingle();
            Container.Bind<IGlobalDataService>().To<GlobalDataService>().AsSingle();
            Container.Bind<InfoBetweenScenes.InfoBetweenScenes>().AsSingle();

            CreateRootUI();
            
            Container.Bind<IPopupProvider>().To<ResourcesPopupProvider>().AsSingle();
            Container.Bind<IPopupFactory>().To<PopupFactory>().AsSingle();
            Container.Bind(typeof(IPopupService), typeof(IGeneralRestartable)).To<PopupService>().AsSingle();

            
            BindProjectStateMachine();
            
            Container.BindInterfacesAndSelfTo<ProjectInitializer>().AsSingle().WithArguments(Localisation);
        }

        private void CreateRootUI()
        {
            RootUIViewProvider rootUI = Container.InstantiatePrefabForComponent<RootUIViewProvider>(RootUIPrefab);

            Container.Bind<RootUIViewProvider>().FromInstance(rootUI).AsSingle();
            Container.Bind<IBackPopupPlane>().FromInstance(rootUI.BackPopupPlane).AsSingle();
            Container.Bind<ITransformable>().FromInstance(rootUI.PopupUpViewProvider).AsSingle();
            
            ILoadingScreen loadingScreen = Container.InstantiatePrefabForComponent<ILoadingScreen>(LoadingScreenPrefab, Vector3.zero, Quaternion.identity, rootUI.LoadingCanvasGroup.transform);

            loadingScreen.RectTransform.offsetMin = Vector2.zero;
            loadingScreen.RectTransform.offsetMax = Vector2.zero;
            loadingScreen.RectTransform.localScale = Vector3.one;
            loadingScreen.RectTransform.anchoredPosition3D = Vector3.zero;
            
            Container.Bind<ILoadingScreen>().FromInstance(loadingScreen).AsSingle();
        }

        private void BindProjectStateMachine()
        {
            LoadingSceneState loadingSceneState = Container.Instantiate<LoadingSceneState>(new[] { Container.Resolve<ILoadingScreen>() });
            
            IEnumerable<IExitableState> enumerable = new[] { loadingSceneState };
            IStateMachine stateMachine = new StateMachine();
            stateMachine.AsyncInitialize(enumerable);
            
            Container.Bind<IStateMachine>().FromInstance(stateMachine).AsSingle();
        }
    }
}