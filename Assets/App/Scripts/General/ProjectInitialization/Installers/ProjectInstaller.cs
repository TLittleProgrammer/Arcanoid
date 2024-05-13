using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.SceneManagment;
using App.Scripts.General.Components;
using App.Scripts.General.Constants;
using App.Scripts.General.DotweenContainerService;
using App.Scripts.General.Levels;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.Popup;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.General.Popup.Factory;
using App.Scripts.General.ProjectInitialization.Settings;
using App.Scripts.General.RootUI;
using App.Scripts.General.States;
using App.Scripts.General.UserData.Services;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.ProjectInitialization.Installers
{
    public class ProjectInstaller : MonoInstaller, IInitializable   
    {
        public RootUIViewProvider RootUIPrefab;
        public GameObject LoadingScreenPrefab;
        
        [Inject] private ApplicationSettings _applicationSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProjectInstaller>().FromInstance(this).AsSingle();
            Container.Bind<ISceneManagementService>().To<SceneManagementService>().AsSingle();
            Container.Bind<IDotweenContainerService>().To<DotweenContainerService.DotweenContainerService>().AsSingle();
            Container.Bind<LevelProgressDataService>().AsSingle();
            Container.Bind<ILevelPackTransferData>().To<LevelPackTransferDataService>().AsSingle();

            BindPopups();
            
            CreateRootUI();
        }

        private void BindPopups()
        {
            
        }

        public void Initialize()
        {
            Application.targetFrameRate = _applicationSettings.TargetFPS;
            QualitySettings.vSyncCount = _applicationSettings.VSyncCounter;
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
            BindProjectStateMachine(loadingScreen);
        }

        private void BindProjectStateMachine(ILoadingScreen loadingScreen)
        {
            LoadingSceneState loadingSceneState = Container.Instantiate<LoadingSceneState>(new[] { loadingScreen });
            
            IEnumerable<IExitableState> enumerable = new[] { loadingSceneState };
            IStateMachine stateMachine = new StateMachine();
            stateMachine.AsyncInitialize(enumerable);
            
            Container.Bind<IStateMachine>().FromInstance(stateMachine).AsSingle();
        }
    }
}