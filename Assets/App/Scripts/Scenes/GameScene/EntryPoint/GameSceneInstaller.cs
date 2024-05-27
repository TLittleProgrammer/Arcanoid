using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.External.Extensions.ZenjectExtensions;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers;
using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Ball.Collision;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Blocks;
using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Boosts.UI;
using App.Scripts.Scenes.GameScene.Features.Camera;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;
using App.Scripts.Scenes.GameScene.Features.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels;
using App.Scripts.Scenes.GameScene.Features.LevelView;
using App.Scripts.Scenes.GameScene.Features.MiniGun;
using App.Scripts.Scenes.GameScene.Features.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.PlayerShape.Collisions;
using App.Scripts.Scenes.GameScene.Features.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Pools;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Restart;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.TopSprites;
using App.Scripts.Scenes.GameScene.Features.Walls;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private BoostItemView _boostItemViewPrefab;
        [SerializeField] private BoostsViewContainer _boostsViewContainer;
        [SerializeField] private Image _menuButton;

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
            Container.Bind<IBlockShakeService>().To<BlockShakeService>().AsSingle();
            FactoriesInstaller.Install(Container, _rootUIView, _boostItemViewPrefab);
            
            Container.Bind<IRectMousePositionChecker>().To<RectMousePositionChecker>().AsSingle().WithArguments(_rectTransformableViews);
            Container.BindInterfacesAndSelfTo<LevelProgressService>().AsSingle().WithArguments(_levelPackInfoView, _levelPackBackground);
            Container.BindInterfacesAndSelfTo<BulletPositionChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<MiniGunService>().AsSingle();
            Container.Bind<ICameraService>().To<CameraService>().AsSingle().WithArguments(_camera);

            ScreenInfoProviderInstaller.Install(Container);
            InputInstaller.Install(Container);
            LevelServicesInstaller.Install(Container);
            
            Container.Bind<IGridPositionResolver>().To<GridPositionResolver>().AsSingle().WithArguments(_header);
            Container.Bind<IBallSpeedUpdater>().To<BallSpeedUpdater>().AsSingle();
            Container.Bind<IShapePositionChecker>().To<PlayerShapePositionChecker>().AsSingle().WithArguments(_playerShape);
            Container.Bind<PlayerCollisionService>().AsSingle().WithArguments(_playerShape).NonLazy();
            
            BindPlayerMoving();
            BindHealthPointService();
            BindBallMovers();
            
            Container.Bind<IBallMovementService>().To<BallMovementService>().AsSingle().WithArguments(_ballView);
            Container.Bind<IBallCollisionService>().To<BallCollisionService>().AsSingle().WithArguments(_ballView).NonLazy();
            Container.Bind<IWallLoader>().To<WallLoader>().AsSingle().WithArguments(_wallPrefab);
            Container.Bind<IRestartService>().To<RestartService>().AsSingle();
            Container.Bind<IItemViewService>().To<ItemViewService>().AsSingle();
            
            ShowLevelAnimationInstaller.Install(Container);
            ItemsDestroyableInstaller.Install(Container);
            BehaviourTreeInstaller.Install(Container);
            StateMachineInstaller.Install(Container, _gameLoopTickables, _projectStateMachine, _restartables, _levelPackInfoView, _restartablesForLoadNewLevel);
            
            Container.BindInterfacesTo<BehaviourTreeInitializer>().AsSingle();
            Container.BindInterfacesTo<ItemsDestroyerInitializer>().AsSingle();
            Container.BindInterfacesTo<GameInitializer>().AsSingle();
            Container.BindInterfacesTo<InitializeAllRestartableAnsTickablesLists>().AsSingle().WithArguments(_restartables, _restartablesForLoadNewLevel, _gameLoopTickables);
        }

        private void BindInitializeDependencies()
        {
            Container.Bind<TextAsset>().FromInstance(_levelData).AsSingle();
            Container.Bind<PlayerView>().FromInstance(_playerShape).AsSingle();
            Container.Bind<BallView>().FromInstance(_ballView).AsSingle();
            Container.Bind<Image>().FromInstance(_menuButton).AsSingle();
            Container.Bind<BoostsViewContainer>().FromInstance(_boostsViewContainer).AsSingle();
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
            BindPool<BoostView, BoostView.Pool>(PoolTypeId.Boosts);
            BindPool<BulletView, BulletView.Pool>(PoolTypeId.Bullets);
            BindPool<BulletEffectView, BulletEffectView.Pool>(PoolTypeId.BulletEffect);
        }

        private void BindPool<TInstance, TPool>(PoolTypeId poolType) where TPool : IMemoryPool where TInstance : MonoBehaviour 
        {
            Container.BindPool<TInstance, TPool>(_poolProviders.Pools[poolType].InitialSize, _poolProviders.Pools[poolType].View.GetComponent<TInstance>(), _poolProviders.Pools[poolType].ParentName);
        }
    }
}