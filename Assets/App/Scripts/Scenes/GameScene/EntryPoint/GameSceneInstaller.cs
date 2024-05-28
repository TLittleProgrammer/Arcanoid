using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.Components;
using App.Scripts.External.Extensions.ZenjectExtensions;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.EntryPoint.Bootstrap;
using App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers;
using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Ball.Collision;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Blocks;
using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Boosts.UI;
using App.Scripts.Scenes.GameScene.Features.Camera;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Damage;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;
using App.Scripts.Scenes.GameScene.Features.Helpers;
using App.Scripts.Scenes.GameScene.Features.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels;
using App.Scripts.Scenes.GameScene.Features.Levels.Animations;
using App.Scripts.Scenes.GameScene.Features.LevelView;
using App.Scripts.Scenes.GameScene.Features.MiniGun;
using App.Scripts.Scenes.GameScene.Features.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.PlayerShape.Collisions;
using App.Scripts.Scenes.GameScene.Features.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Pools;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Restart;
using App.Scripts.Scenes.GameScene.Features.ScoreAnimation;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using App.Scripts.Scenes.GameScene.Features.ServiceActivator;
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
        [SerializeField] private Camera _camera;
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

            Container.Bind<ITweenersLocator>().To<TweenersLocator>().AsSingle();
            Container.Bind<IScoreAnimationService>().To<ScoreAnimationService>().AsSingle();
            Container.Bind<IShakeService>().To<ShakeService>().AsSingle();
            
            TimeProviderInstaller.Install(Container);
            PoolsInstaller.Install(Container, _poolProviders);
            FactoriesInstaller.Install(Container, _rootUIView, _boostItemViewPrefab);
            
            Container.Bind<IRectMousePositionChecker>().To<RectMousePositionChecker>().AsSingle().WithArguments(_rectTransformableViews.ToList());
            Container.Bind<ICameraService>().To<CameraService>().AsSingle().WithArguments(_camera);
            Container.Bind<IScreenInfoProvider>().To<ScreenInfoProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletPositionChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<MiniGunService>().AsSingle();

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
            Container.Bind<IServicesActivator>().To<ServiceActivator>().AsSingle();
            Container.Bind<IShowLevelAnimation>().To<SimpleShowLevelAnimation>().AsSingle();
            Container.Bind<IGetDamageService>().To<GetDamageService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelProgressService>().AsSingle().WithArguments(_levelPackInfoView, _levelPackBackground);
            Container.Bind<GameLoopSubscriber>().AsSingle();
            
            ItemsDestroyableInstaller.Install(Container);
            BehaviourTreeInstaller.Install(Container);
            StateMachineInstaller.Install(Container, _gameLoopTickables, _projectStateMachine, _restartables, _levelPackInfoView, _restartablesForLoadNewLevel);
            
            Container.Bind<List<IActivable>>().FromMethod(ctx => ctx.Container.ResolveAll<IActivable>().ToList()).AsSingle();
            
            Container.BindInterfacesTo<GameBootstrapper>().AsSingle();
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
            Container.Bind<IViewHealthPointService>().To<ViewHealthPointService>().AsSingle().WithArguments(_healthParent as ITransformable);
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
    }
}