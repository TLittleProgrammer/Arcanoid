using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.Components;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.RootUI;
using App.Scripts.Scenes.GameScene.EntryPoint.Bootstrap;
using App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.UI;
using App.Scripts.Scenes.GameScene.Features.Boosts.MiniGun;
using App.Scripts.Scenes.GameScene.Features.Camera;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.Damage;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Collisions;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Entities.Walls;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;
using App.Scripts.Scenes.GameScene.Features.Helpers;
using App.Scripts.Scenes.GameScene.Features.Levels;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Animations;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelView;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading;
using App.Scripts.Scenes.GameScene.Features.Levels.SkipLevel;
using App.Scripts.Scenes.GameScene.Features.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Restart;
using App.Scripts.Scenes.GameScene.Features.ScoreAnimation;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using App.Scripts.Scenes.GameScene.Features.ServiceActivator;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Shake;
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
        [SerializeField] private LevelPackInfoView _levelPackInfoView;
        [SerializeField] private LevelPackBackgroundView _levelPackBackground;
        [SerializeField] private HealthPointViewParent _healthParent;
        [SerializeField] private List<RectTransformableView> _rectTransformableViews;
        [SerializeField] private WallView _wallPrefab;
        [SerializeField] private BoostItemView _boostItemViewPrefab;
        [SerializeField] private BoostsViewContainer _boostsViewContainer;
        [SerializeField] private Image _menuButton;
        [SerializeField] private Button _skipLevelButton;

        [Inject] private PoolProviders _poolProviders;
        [Inject] private IStateMachine _projectStateMachine;
        [Inject] private RootUIViewProvider _rootUIView;

        private readonly List<IRestartable> _restartables = new();
        private readonly List<IRestartable> _restartablesForLoadNewLevel = new();
        private readonly List<ITickable> _gameLoopTickables = new();

        public override void InstallBindings()
        {
            BindInitializeDependencies();

            TimeProviderInstaller.Install(Container);
            PoolsInstaller.Install(Container, _poolProviders);
            FactoriesInstaller.Install(Container, _rootUIView, _boostItemViewPrefab);
            InputInstaller.Install(Container);
            LevelServicesInstaller.Install(Container);
            EntityDestroyableInstaller.Install(Container);
            BehaviourTreeInstaller.Install(Container);
            
            Container.Bind<List<IActivable>>().FromMethod(ctx => ctx.Container.ResolveAll<IActivable>().ToList()).AsSingle();

            Container.Bind<ITweenersLocator>().To<TweenersLocator>().AsSingle();
            Container.Bind<IScoreAnimationService>().To<ScoreAnimationService>().AsSingle();
            Container.Bind<IShakeService>().To<ShakeService>().AsSingle();
            Container.Bind<IRectMousePositionChecker>().To<RectMousePositionChecker>().AsSingle().WithArguments(_rectTransformableViews.ToList());
            Container.Bind<ICameraService>().To<CameraService>().AsSingle().WithArguments(_camera);
            Container.Bind<IScreenInfoProvider>().To<ScreenInfoProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<BulletPositionChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<MiniGunService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BirdsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BirdRespawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BirdsHealthContainer>().AsSingle();
            Container.BindInterfacesAndSelfTo<MechanicsByLevelActivator>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelDataChooser>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelLoadService>().AsSingle();

            Container.Bind<IGridPositionResolver>().To<GridPositionResolver>().AsSingle().WithArguments(_header);
            Container.Bind<IShapePositionChecker>().To<PlayerShapePositionChecker>().AsSingle().WithArguments(_playerShape);
            Container.Bind<PlayerCollisionService>().AsSingle().WithArguments(_playerShape).NonLazy();
            
            BindPlayerMoving();
            BindHealthPointService();
            
            Container.Bind<IWallLoader>().To<WallLoader>().AsSingle().WithArguments(_wallPrefab);
            Container.Bind<IRestartService>().To<RestartService>().AsSingle();
            Container.Bind<IEntityViewService>().To<EntityViewService>().AsSingle();
            Container.Bind<IServicesActivator>().To<ServiceActivator>().AsSingle();
            Container.Bind<IShowLevelAnimation>().To<SimpleShowLevelAnimation>().AsSingle();
            Container.Bind<IGetDamageService>().To<GetDamageService>().AsSingle();
            Container.Bind(typeof(IActivable), typeof(IBallsService)).To<BallsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelProgressService>().AsSingle().WithArguments(_levelPackInfoView, _levelPackBackground);

            Container.Bind<SkipLevelService>().AsSingle().WithArguments(_skipLevelButton).NonLazy();
            
            StateMachineInstaller.Install(Container, _gameLoopTickables, _projectStateMachine, _restartables, _levelPackInfoView, _restartablesForLoadNewLevel);
            
            Container.BindInterfacesTo<GameBootstrapper>().AsSingle();
        }

        private void BindInitializeDependencies()
        {
            Container.Bind<TextAsset>().FromInstance(_levelData).AsSingle();
            Container.Bind<PlayerView>().FromInstance(_playerShape).AsSingle();
            Container.Bind<Image>().FromInstance(_menuButton).AsSingle();
            Container.Bind<BoostsViewContainer>().FromInstance(_boostsViewContainer).AsSingle();
        }

        private void BindHealthPointService()
        {
            Container.Bind<IViewHealthPointService>().To<ViewHealthPointService>().AsSingle().WithArguments(_healthParent as ITransformable);
            Container.Bind<IHealthContainer>().To<HealthContainer>().AsSingle();
        }

        private void BindPlayerMoving()
        {
            ShapeMoverSettings shapeMoverSettings = new();
            shapeMoverSettings.Speed = 5f;
            
            Container.Bind<IPlayerShapeMover>().To<PlayerShapeMover>().AsSingle().WithArguments(_playerShape, shapeMoverSettings);
        }
    }
}