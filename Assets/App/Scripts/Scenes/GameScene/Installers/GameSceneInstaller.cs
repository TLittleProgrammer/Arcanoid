using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.External.Extensions.ZenjectExtensions;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.Popup;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.General.Popup.Factory;
using App.Scripts.Scenes.GameScene.Ball;
using App.Scripts.Scenes.GameScene.Ball.Collision;
using App.Scripts.Scenes.GameScene.Ball.Movement;
using App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Camera;
using App.Scripts.Scenes.GameScene.Constants;
using App.Scripts.Scenes.GameScene.Dotween;
using App.Scripts.Scenes.GameScene.Effects;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Factories.CircleEffect;
using App.Scripts.Scenes.GameScene.Factories.Entity;
using App.Scripts.Scenes.GameScene.Factories.Health;
using App.Scripts.Scenes.GameScene.Factories.OnTopSprite;
using App.Scripts.Scenes.GameScene.Grid;
using App.Scripts.Scenes.GameScene.Healthes;
using App.Scripts.Scenes.GameScene.Healthes.View;
using App.Scripts.Scenes.GameScene.Input;
using App.Scripts.Scenes.GameScene.LevelProgress;
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
using App.Scripts.Scenes.GameScene.TopSprites;
using App.Scripts.Scenes.GameScene.Walls;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private RectTransform _header;
        [SerializeField] private TextAsset _levelData;
        [SerializeField] private PlayerView _playerShape;
        [SerializeField] private BallView _ballView;
        [SerializeField] private LevelPackInfoView _levelPackInfoView;
        [SerializeField] private LevelPackBackgroundView _levelPackBackground;
        [SerializeField] private HealthPointViewParent _healthParent;
        [SerializeField] private List<RectTransformableView> _rectTransformableViews;
        [SerializeField] private WallView _wallPrefab;

        [Inject] private PoolProviders _poolProviders;
        [Inject] private IStateMachine _projectStateMachine;

        private List<IRestartable> _restartables = new();
        private List<IRestartable> _restartablesForLoadNewLevel = new();
        private List<ITickable> _gameLoopTickables = new();
        
        private IStateMachine _gameStateMachine = new StateMachine();
        
        public override void InstallBindings()
        {
            BindInitializeDependencies();
            BindTweenersLocator();
            BindTimeProvider();
            BindScoreAnimationService();
            BindPools();
            BindPoolContainer();
            BindFactories();
            BindMousePositionChecker();
            BindLevelProgressService();
            BindCameraService();
            BindScreenInfoProvider();
            BindInput();
            BindGridPositionResolver();
            BindBallSpeedUpdater();
            BindLevelLoader();
            BindPositionCheckers();
            BindPlayerMoving();
            BindHealthPointService();
            BindBallMovers();
            BindBallMovement();
            BindBallCollisionService();
            BindWallLoader();

            BindGameStateMachine();
            
            Container.BindInterfacesTo<GameInitializer>().AsSingle();
        }

        private void BindBallCollisionService()
        {
            Container
                .Bind<IBallCollisionService>()
                .To<BallCollisionService>()
                .AsSingle()
                .WithArguments(_ballView, _playerShape)
                .NonLazy();
        }

        private void BindWallLoader()
        {
            Container.Bind<IWallLoader>().To<WallLoader>().AsSingle().WithArguments(_wallPrefab);
        }

        private void BindInitializeDependencies()
        {
            Container.Bind<TextAsset>().FromInstance(_levelData).AsSingle();
            Container.Bind<PlayerView>().FromInstance(_playerShape).AsSingle();
        }

        private void BindBallSpeedUpdater()
        {
            Container.Bind<IBallSpeedUpdater>().To<BallSpeedUpdater>().AsSingle();
        }

        private void BindMousePositionChecker()
        {
            Container.Bind<IRectMousePositionChecker>().To<RectMousePositionChecker>().AsSingle().WithArguments(_rectTransformableViews);
        }

        private void BindHealthPointService()
        {
            Container.Bind<IHealthPointService>().To<HealthPointService>().AsSingle().WithArguments(_healthParent as ITransformable);
            Container.Bind<IHealthContainer>().To<HealthContainer>().AsSingle();
            
            _restartables.Add(Resolve<IHealthPointService>());
            _restartablesForLoadNewLevel.Add(Resolve<IHealthPointService>());
            
            _restartables.Add(Resolve<IHealthContainer>());
            _restartablesForLoadNewLevel.Add(Resolve<IHealthContainer>());
        }

        private void BindTweenersLocator()
        {
            Container.Bind<ITweenersLocator>().To<TweenersLocator>().AsSingle();
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

            _restartables.Add(Container.Resolve<ILevelProgressService>());
            _restartablesForLoadNewLevel.Add(Container.Resolve<ILevelProgressService>());
        }

        private void BindGameStateMachine()
        {
            GameLoopState gameLoopState = Container.Instantiate<GameLoopState>(new object[]{_gameLoopTickables, _gameStateMachine});
            PopupState popupState = Container.Instantiate<PopupState>();
            LooseState looseState = Container.Instantiate<LooseState>();
            WinState winState = Container.Instantiate<WinState>();
            RestartState restartState = Container.Instantiate<RestartState>(new object[] {_restartables, _gameStateMachine});
            LoadNextLevelState loadNextLevelState = Container.Instantiate<LoadNextLevelState>(new object[] {_levelPackInfoView, _restartablesForLoadNewLevel, _gameStateMachine});

            Container.BindInterfacesTo<GameLoopState>().FromInstance(gameLoopState);
            
            
            _gameStateMachine.AsyncInitialize(new IState[] { gameLoopState, popupState, restartState, loadNextLevelState, looseState, winState });
            _gameStateMachine.Enter<GameLoopState>();
            
            Container.Bind<IStateMachine>()
                .WithId(BindingConstants.GameStateMachine)
                .FromInstance(_gameStateMachine)
                .AsCached()
                .NonLazy();

            Container.Bind<IStateMachine>()
                .WithId(BindingConstants.ProjectStateMachine)
                .FromInstance(_projectStateMachine)
                .AsCached()
                .NonLazy();
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
                .WithArguments(_ballView);
        }

        private void BindPositionCheckers()
        {
            Container
                .Bind<IShapePositionChecker>()
                .To<PlayerShapePositionChecker>()
                .AsTransient()
                .WithArguments(_playerShape);
        }

        private void BindPlayerMoving()
        {
            ShapeMoverSettings shapeMoverSettings = new();
            shapeMoverSettings.Speed = 5f;
            
            Container.Bind<IPlayerShapeMover>().To<PlayerShapeMover>().AsSingle().WithArguments(_playerShape, shapeMoverSettings);

            var mover = Resolve<IPlayerShapeMover>();
            _restartables.Add(mover);
            _restartablesForLoadNewLevel.Add(mover);
            _gameLoopTickables.Add(mover);
        }

        private void BindBallMovement()
        {
            Container.Bind<IBallMovementService>().To<BallMovementService>().AsSingle().WithArguments(_ballView);
            
            _restartables.Add(Resolve<IBallMovementService>());
            _restartablesForLoadNewLevel.Add(Resolve<IBallMovementService>());
            _gameLoopTickables.Add(Resolve<IBallMovementService>());
        }

        private void BindFactories()
        {
            Container.BindFactory<string, IEntityView, IEntityView.Factory>().FromFactory<EntityFactory>();
            Container.BindFactory<ITransformable, IHealthPointView, IHealthPointView.Factory>().FromFactory<HealthFactory>();
            Container.BindFactory<EntityView, CircleEffect, CircleEffect.Factory>().FromFactory<CircleEffectFactory>();
            Container.BindFactory<IEntityView, OnTopSprites, OnTopSprites.Factory>().FromFactory<OnTopSpriteFactory>();
            
            Container.Bind<IPopupProvider>().To<ResourcesPopupProvider>().AsSingle();
            Container.Bind<IPopupFactory>().To<PopupFactory>().AsSingle();
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
            BindPool<HealthPointView, HealthPointView.Pool>(PoolTypeId.HealthPointView);
            BindPool<OnTopSprites, OnTopSprites.Pool>(PoolTypeId.OnTopSprite);
        }

        private void BindPool<TInstance, TPool>(PoolTypeId poolType) where TPool : IMemoryPool where TInstance : MonoBehaviour 
        {
            Container.BindPool<TInstance, TPool>(_poolProviders.Pools[poolType].InitialSize, _poolProviders.Pools[poolType].View.GetComponent<TInstance>(), _poolProviders.Pools[poolType].ParentName);
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
            Container.Bind<ITimeScaleAnimator>().To<TimeScaleAnimator>().AsSingle();
            
            _restartables.Add(Container.Resolve<ITimeProvider>());
            _restartablesForLoadNewLevel.Add(Container.Resolve<ITimeProvider>());
        }

        private void BindInput()
        {
            Container.Bind<IClickDetector>().To<ClickDetector>().AsSingle();
            Container.Bind<IInputService>().To<InputService>().AsSingle();
            
            _gameLoopTickables.Add(Resolve<IClickDetector>());
            _gameLoopTickables.Add(Resolve<IInputService>());
        }

        private TResult Resolve<TResult>()
        {
            return Container.Resolve<TResult>();
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