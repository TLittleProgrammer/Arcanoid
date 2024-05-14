using System.Collections.Generic;
using App.Scripts.External.Extensions.ZenjectExtensions;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.Initialization;
using App.Scripts.General.Components;
using App.Scripts.General.Constants;
using App.Scripts.General.Levels;
using App.Scripts.General.Popup;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.General.Popup.Factory;
using App.Scripts.General.UserData.Data;
using App.Scripts.Scenes.GameScene.Ball;
using App.Scripts.Scenes.GameScene.Ball.Movement;
using App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Camera;
using App.Scripts.Scenes.GameScene.Collisions;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Constants;
using App.Scripts.Scenes.GameScene.Containers;
using App.Scripts.Scenes.GameScene.Effects;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Factories.EntityFactory;
using App.Scripts.Scenes.GameScene.Grid;
using App.Scripts.Scenes.GameScene.Infrastructure;
using App.Scripts.Scenes.GameScene.Input;
using App.Scripts.Scenes.GameScene.LevelProgress;
using App.Scripts.Scenes.GameScene.Levels;
using App.Scripts.Scenes.GameScene.Levels.Load;
using App.Scripts.Scenes.GameScene.Levels.View;
using App.Scripts.Scenes.GameScene.LevelView;
using App.Scripts.Scenes.GameScene.PlayerShape;
using App.Scripts.Scenes.GameScene.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Pools;
using App.Scripts.Scenes.GameScene.PositionChecker;
using App.Scripts.Scenes.GameScene.ScoreAnimation;
using App.Scripts.Scenes.GameScene.ScreenInfo;
using App.Scripts.Scenes.GameScene.Settings;
using App.Scripts.Scenes.GameScene.States;
using App.Scripts.Scenes.GameScene.Time;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Installers
{
    public class GameSceneInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private RectTransform _header;
        [SerializeField] private TextAsset _levelData;
        [SerializeField] private PlayerView _playerShape;
        [SerializeField] private BallView _ballView;
        [SerializeField] private LevelPackInfoView _levelPackInfoView;
        [SerializeField] private LevelPackBackgroundView _levelPackBackground;

        [Inject] private PoolProviders _poolProviders;
        [Inject] private IStateMachine _projectStateMachine;

        private List<IRestartable> _restartables = new();
        private List<IRestartable> _restartablesForLoadNewLevel = new();
        
        private IStateMachine _stateMachine = new StateMachine();

        public async void Initialize()
        {
            LevelData levelData = ChooseLevelData();
            
            Container.Resolve<IGridPositionResolver>().AsyncInitialize(levelData);
            Container.Resolve<IContainer<IBoxColliderable2D>>().AddItem(_playerShape);

            LoadLevel(levelData);
            await Container.Resolve<IPopupProvider>().AsyncInitialize(Pathes.PathToPopups);
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSceneInstaller>().FromInstance(this).AsSingle();

            BindTimeProvider();
            BindScoreAnimationService();
            BindPools();
            BindPoolContainer();
            BindFactories();
            BindLevelProgressService();
            BindCameraService();
            BindScreenInfoProvider();
            BindInput();
            BindGridPositionResolver();
            BindContainers();
            BindLevelLoader();
            BindCollisionService();
            BindPositionCheckers();
            BindPlayerMoving();
            BindBallMovers();
            BindBallMovement();
            
            BindGameStateMachine();
        }

        private void BindScoreAnimationService()
        { 
            Container.Bind<IScoreAnimationService>().To<ScoreAnimationService>().AsSingle();
        }

        private void BindLevelProgressService()
        {
            Container
                .BindInterfacesAndSelfTo<LevelProgressService>()
                .AsSingle()
                .WithArguments(_levelPackInfoView, _levelPackBackground);
            
            Container
                .Bind<IStopGameService>()
                .To<StopGameService>()
                .AsSingle();

            _restartables.Add(Container.Resolve<ILevelProgressService>());
            _restartablesForLoadNewLevel.Add(Container.Resolve<IStopGameService>());
            _restartablesForLoadNewLevel.Add(Container.Resolve<ILevelProgressService>());
        }

        private void BindGameStateMachine()
        {
            GameLoopState gameLoopState = Container.Instantiate<GameLoopState>();
            PopupState popupState = Container.Instantiate<PopupState>();
            RestartState restartState = Container.Instantiate<RestartState>(new object[] {_restartables, _stateMachine});
            LoadNextLevelState loadNextLevelState = Container.Instantiate<LoadNextLevelState>(new object[] {_levelPackInfoView, _restartablesForLoadNewLevel, _stateMachine});
            
            _stateMachine.AsyncInitialize(new IState[] { gameLoopState, popupState, restartState, loadNextLevelState });
            _stateMachine.Enter<GameLoopState>();
            
            Container.Bind<IStateMachine>()
                .WithId(BindingConstants.GameStateMachine)
                .FromInstance(_stateMachine)
                .AsCached()
                .NonLazy();

            Container.Bind<IStateMachine>()
                .WithId(BindingConstants.ProjectStateMachine)
                .FromInstance(_projectStateMachine)
                .AsCached()
                .NonLazy();
        }

        private LevelData ChooseLevelData()
        {
            var transferData = Container.Resolve<ILevelPackTransferData>();
            
            if (transferData.NeedLoadLevel)
            {
                return JsonConvert.DeserializeObject<LevelData>(transferData.LevelPack.Levels[transferData.LevelIndex].text);
            }

            return JsonConvert.DeserializeObject<LevelData>(_levelData.text);
        }

        private void BindCollisionService()
        {
            Container.Bind<ICollisionService<EntityView>>().To<EntityCollisionService>().AsSingle();
        }

        private void BindContainers()
        {
            Container.Bind<IContainer<IBoxColliderable2D>>().To<EntityColliderContainer>().AsSingle();
        }

        private void BindBallMovers()
        {
            Container
                .Bind<IBallFollowMover>()
                .To<BallFollowMover>()
                .AsSingle()
                .WithArguments(_ballView, _playerShape);
            
            Container
                .Bind<IBallFreeFlightMover>()
                .To<BallFreeFlight>()
                .AsSingle()
                .WithArguments(_ballView, _stateMachine);
        }

        private void BindPositionCheckers()
        {
            Container
                .Bind<IShapePositionChecker>()
                .To<PlayerShapePositionChecker>()
                .AsTransient()
                .WithArguments(_playerShape);
            
            Container
                .Bind<IBallPositionChecker>()
                .To<BallPositionChecker>()
                .AsTransient()
                .WithArguments(_ballView);
        }

        private void BindPlayerMoving()
        {
            ShapeMoverSettings shapeMoverSettings = new();
            shapeMoverSettings.Speed = 5f;

            
            Container.BindInterfacesAndSelfTo<PlayerShapeMover>().AsSingle().WithArguments(_playerShape, shapeMoverSettings, _stateMachine);
            
            _restartables.Add(Container.Resolve<PlayerShapeMover>());
            _restartablesForLoadNewLevel.Add(Container.Resolve<PlayerShapeMover>());
        }

        private void BindBallMovement()
        {
            Container.BindInterfacesAndSelfTo<BallMovementService>().AsSingle().WithArguments(_ballView);
            
            _restartables.Add(Container.Resolve<BallMovementService>());
            _restartablesForLoadNewLevel.Add(Container.Resolve<BallMovementService>());
        }

        private void BindFactories()
        {
            Container.BindFactory<string, IEntityView, IEntityView.Factory>().FromFactory<EntityFactory>();
            
            Container.Bind<IPopupProvider>().To<ResourcesPopupProvider>().AsSingle();
            Container
                .BindFactory<PopupTypeId, ITransformable, IViewPopupProvider, IViewPopupProvider.Factory>()
                .FromFactory<PopupFactory>();

            Container.Bind<IPopupService>().To<PopupService>().AsSingle();
            
            _restartables.Add(Container.Resolve<IPopupService>() as IRestartable);
            _restartablesForLoadNewLevel.Add(Container.Resolve<IPopupService>() as IRestartable);
        }

        private void BindPoolContainer()
        {
            Container.Bind<IPoolContainer>().To<PoolContainer>().AsSingle();
            
            _restartables.Add(Container.Resolve<IPoolContainer>());
            _restartablesForLoadNewLevel.Add(Container.Resolve<IPoolContainer>());
        }

        private void BindPools()
        {
            BindPool<EntityView, EntityView.Pool>(PoolTypeId.EntityView);
            BindPool<CircleEffect, IEffect<CircleEffect>.Pool>(PoolTypeId.CircleEffect);
        }

        private void BindPool<TInstance, TPool>(PoolTypeId poolType) where TPool : IMemoryPool where TInstance : MonoBehaviour 
        {
            Container.BindPool<TInstance, TPool>(_poolProviders.Pools[poolType].InitialSize, (TInstance)_poolProviders.Pools[poolType].View, _poolProviders.Pools[poolType].ParentName);
        }

        private void BindLevelLoader()
        {
            Container.Bind<ILevelViewUpdater>().To<LevelViewUpdater>().AsSingle();
            Container.Bind<ILevelLoader>().To<LevelLoader>().AsSingle();
            
            _restartables.Add(Container.Resolve<ILevelLoader>());
        }

        private void BindTimeProvider()
        {
            Container.Bind<ITimeProvider>().To<TimeProvider>().AsSingle();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<ClickDetector>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
        }

        private void LoadLevel(LevelData levelData)
        {
            var levelLoader  = Container.Resolve<ILevelLoader>();

            levelLoader.LoadLevel(levelData);
            Container.Resolve<ILevelProgressService>().CalculateStepByLevelData(levelData);
        }

        private void BindGridPositionResolver()
        {
            Container.Bind<IGridPositionResolver>().To<GridPositionResolver>().AsSingle().WithArguments(_header);
            
            _restartables.Add(Container.Resolve<IGridPositionResolver>());
        }

        private void BindCameraService()
        {
            Container.Bind<ICameraService>().To<CameraService>().AsSingle().WithArguments(_camera);
        }

        private void BindScreenInfoProvider()
        {
            Container.Bind<IScreenInfoProvider>().To<ScreenInfoProvider>().AsSingle();
        }
    }
}