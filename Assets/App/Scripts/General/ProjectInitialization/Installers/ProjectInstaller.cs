using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.SceneManagment;
using App.Scripts.General.DotweenContainerService;
using App.Scripts.General.Levels;
using App.Scripts.General.LoadingScreen;
using App.Scripts.General.ProjectInitialization.Settings;
using App.Scripts.General.UserData.Services;
using App.Scripts.Scenes.Bootstrap.States;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.ProjectInitialization.Installers
{
    public class ProjectInstaller : MonoInstaller, IInitializable   
    {
        public Canvas RootUIPrefab;
        public GameObject LoadingScreenPrefab;
        
        [Inject] private ApplicationSettings _applicationSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProjectInstaller>().FromInstance(this).AsSingle();
            Container.Bind<ISceneManagementService>().To<SceneManagementService>().AsSingle();
            Container.Bind<IDotweenContainerService>().To<DotweenContainerService.DotweenContainerService>().AsSingle();
            Container.Bind<LevelProgressDataService>().AsSingle();
            Container.Bind<ILevelPackTransferData>().To<LevelPackTransferDataService>().AsSingle();
            
            CreateRootUI();
        }

        public void Initialize()
        {
            Application.targetFrameRate = _applicationSettings.TargetFPS;
            QualitySettings.vSyncCount = _applicationSettings.VSyncCounter;
        }

        private void CreateRootUI()
        {
            GameObject rootUI = Container.InstantiatePrefab(RootUIPrefab);

            Container.Bind<CanvasGroup>().FromInstance(rootUI.GetComponent<CanvasGroup>()).AsTransient();
            
            ILoadingScreen loadingScreen = Container.InstantiatePrefabForComponent<ILoadingScreen>(LoadingScreenPrefab, Vector3.zero, Quaternion.identity, rootUI.transform);

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
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle().WithArguments(enumerable);
        }
    }
}