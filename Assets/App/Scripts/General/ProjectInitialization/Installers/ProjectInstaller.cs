using System.Collections.Generic;
using App.Scripts.External.DotweenContainerService;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.Localisation;
using App.Scripts.External.SceneManagment;
using App.Scripts.General.DateTime;
using App.Scripts.General.Energy;
using App.Scripts.General.Levels;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.RootUI;
using App.Scripts.General.States;
using App.Scripts.General.Ticker;
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
            Container.BindInterfacesAndSelfTo<ProjectInitializer>().AsSingle();

            Container.BindInterfacesAndSelfTo<TimeTicker>().AsSingle();
            Container.Bind<IDateTimeService>().To<DateTimeService>().AsSingle();
            Container.Bind<ISceneManagementService>().To<SceneManagementService>().AsSingle();
            Container.Bind<IDotweenContainerService>().To<DotweenContainerService>().AsSingle();
            Container.Bind<LevelProgressDataService>().AsSingle();
            Container.Bind<ILevelPackTransferData>().To<LevelPackTransferData>().AsSingle();
            Container.Bind<ILocaleService>().To<LocaleService>().AsSingle().WithArguments(Localisation.text).NonLazy();
            Container.Bind<EnergyService>().AsSingle().Lazy();

            CreateRootUI();
            BindProjectStateMachine();
        }

        private void CreateRootUI()
        {
            RootUIViewProvider rootUI = Container.InstantiatePrefabForComponent<RootUIViewProvider>(RootUIPrefab);

            Container.Bind<RootUIViewProvider>().FromInstance(rootUI).AsSingle();
            
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