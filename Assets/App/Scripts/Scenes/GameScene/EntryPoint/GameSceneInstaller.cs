using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.External.Extensions.ZenjectExtensions;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.Ball;
using App.Scripts.Scenes.GameScene.Ball.Collision;
using App.Scripts.Scenes.GameScene.Ball.Movement;
using App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Camera;
using App.Scripts.Scenes.GameScene.Effects;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers;
using App.Scripts.Scenes.GameScene.Grid;
using App.Scripts.Scenes.GameScene.Healthes;
using App.Scripts.Scenes.GameScene.Healthes.View;
using App.Scripts.Scenes.GameScene.Installers;
using App.Scripts.Scenes.GameScene.LevelProgress;
using App.Scripts.Scenes.GameScene.LevelView;
using App.Scripts.Scenes.GameScene.PlayerShape;
using App.Scripts.Scenes.GameScene.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Pools;
using App.Scripts.Scenes.GameScene.PositionChecker;
using App.Scripts.Scenes.GameScene.Restart;
using App.Scripts.Scenes.GameScene.Settings;
using App.Scripts.Scenes.GameScene.TopSprites;
using App.Scripts.Scenes.GameScene.Walls;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint
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
        [Inject] private RootUIViewProvider _rootUIView;

        private readonly List<IRestartable> _restartables = new();
        private readonly List<IRestartable> _restartablesForLoadNewLevel = new();
        private readonly List<ITickable> _gameLoopTickables = new();

        public override void InstallBindings()
        {
            BindInitializeDependencies();
            
            TweenersLocatorInstaller.Install(Container);
            TimeProviderInstaller.Install(Container);
            ScoreAnimationServiceInstaller.Install(Container);
            
            BindPools();
            
            Container.Bind<IPoolContainer>().To<PoolContainer>().AsSingle();
            FactoriesInstaller.Install(Container, _rootUIView);
            
            Container.Bind<IRectMousePositionChecker>().To<RectMousePositionChecker>().AsSingle().WithArguments(_rectTransformableViews);
            Container.BindInterfacesAndSelfTo<LevelProgressService>().AsSingle().WithArguments(_levelPackInfoView, _levelPackBackground);
            Container.Bind<ICameraService>().To<CameraService>().AsSingle().WithArguments(_camera);

            ScreenInfoProviderInstaller.Install(Container);
            InputInstaller.Install(Container);
            LevelServicesInstaller.Install(Container);
            
            Container.Bind<IGridPositionResolver>().To<GridPositionResolver>().AsSingle().WithArguments(_header);
            Container.Bind<IBallSpeedUpdater>().To<BallSpeedUpdater>().AsSingle();
            Container.Bind<IShapePositionChecker>().To<PlayerShapePositionChecker>().AsTransient().WithArguments(_playerShape);
            
            BindPlayerMoving();
            BindHealthPointService();
            BindBallMovers();
            
            Container.Bind<IBallMovementService>().To<BallMovementService>().AsSingle().WithArguments(_ballView);
            Container.Bind<IBallCollisionService>().To<BallCollisionService>().AsSingle().WithArguments(_ballView, _playerShape).NonLazy();
            Container.Bind<IWallLoader>().To<WallLoader>().AsSingle().WithArguments(_wallPrefab);
            Container.Bind<IRestartService>().To<RestartService>().AsSingle();
            
            StateMachineInstaller.Install(Container, _gameLoopTickables, _projectStateMachine, _restartables, _levelPackInfoView, _restartablesForLoadNewLevel);
            
            Container.BindInterfacesTo<GameInitializer>().AsSingle();
            Container.BindInterfacesTo<InitializeAllRestartableAnsTickablesLists>().AsSingle().WithArguments(_restartables, _restartablesForLoadNewLevel, _gameLoopTickables);
        }

        private void BindInitializeDependencies()
        {
            Container.Bind<TextAsset>().FromInstance(_levelData).AsSingle();
            Container.Bind<PlayerView>().FromInstance(_playerShape).AsSingle();
        }

        private void BindHealthPointService()
        {
            Container.Bind<IHealthPointService>().To<HealthPointService>().AsSingle().WithArguments(_healthParent as ITransformable);
            Container.Bind<IHealthContainer>().To<HealthContainer>().AsSingle();
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

        private void BindPlayerMoving()
        {
            ShapeMoverSettings shapeMoverSettings = new();
            shapeMoverSettings.Speed = 5f;
            
            Container.Bind<IPlayerShapeMover>().To<PlayerShapeMover>().AsSingle().WithArguments(_playerShape, shapeMoverSettings);
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
    }
}