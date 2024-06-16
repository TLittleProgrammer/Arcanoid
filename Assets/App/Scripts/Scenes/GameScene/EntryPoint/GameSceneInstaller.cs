using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.Components;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.UserData;
using App.Scripts.General.Game;
using App.Scripts.General.UserData.Constants;
using App.Scripts.Scenes.GameScene.EntryPoint.Bootstrap;
using App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers;
using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.UI;
using App.Scripts.Scenes.GameScene.Features.Camera;
using App.Scripts.Scenes.GameScene.Features.Damage;
using App.Scripts.Scenes.GameScene.Features.Dotween;
using App.Scripts.Scenes.GameScene.Features.Effects;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Systems;
using App.Scripts.Scenes.GameScene.Features.Entities.Bird;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Collisions;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.PositionChecker;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using App.Scripts.Scenes.GameScene.Features.Entities.View.Collisions;
using App.Scripts.Scenes.GameScene.Features.Entities.Walls;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Healthes.View;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelView;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.SkipLevel;
using App.Scripts.Scenes.GameScene.Features.Popups.Buttons;
using App.Scripts.Scenes.GameScene.Features.Restart;
using App.Scripts.Scenes.GameScene.Features.ScoreAnimation;
using App.Scripts.Scenes.GameScene.Features.ScreenInfo;
using App.Scripts.Scenes.GameScene.Features.ServiceActivator;
using App.Scripts.Scenes.GameScene.Features.Settings;
using App.Scripts.Scenes.GameScene.Features.Shake;
using App.Scripts.Scenes.GameScene.MVVM.Header;
using App.Scripts.Scenes.GameScene.States.Gameloop;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using LevelProgressSaveService = App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress.LevelProgressSaveService;

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
        [SerializeField] private OpenMenuPopupButton _openMenuPopupButton;

        [Inject] private PoolProviders _poolProviders;
        [Inject] private IStateMachine _projectStateMachine;
        [Inject] private EffectsPrefabProvider _effectsPrefabProvider;
        [Inject] private BoostSettingsContainer _boostSettingsContainer;
        [Inject] private EntityDestroySettings _entityDestroySettings;

        public override void InstallBindings()
        {
            BindInitializeDependencies();

            TimeProviderInstaller.Install(Container);
            PoolsInstaller.Install(Container, _poolProviders, _effectsPrefabProvider);
            FactoriesInstaller.Install(Container, _boostItemViewPrefab);
            InputInstaller.Install(Container);
            LevelServicesInstaller.Install(Container);
            EntityDestroyableInstaller.Install(Container, _entityDestroySettings);
            BehaviourTreeInstaller.Install(Container);
            CommandsInstaller.Install(Container);
            MVVMInstaller.Install(Container, _openMenuPopupButton);
            MiniGunInstaller.Install(Container);
            ConditionsInstaller.Install(Container);
            ShowLevelInstaller.Install(Container);
            BoostBinder.Install(Container, _boostSettingsContainer);
            BirdsInstaller.Install(Container);

            Container.Bind<ICameraService>().To<CameraService>().AsSingle().WithArguments(_camera);
            Container.BindInterfacesAndSelfTo<ScreenInfoProvider>().AsSingle();

            Container.BindInterfacesAndSelfTo<TweenersLocator>().AsSingle();
            Container.Bind<IScoreAnimationService>().To<ScoreAnimationService>().AsSingle();
            Container.Bind<IShakeService>().To<ShakeService>().AsSingle();
            Container.Bind<IRectMousePositionChecker>().To<RectMousePositionChecker>().AsSingle().WithArguments(_rectTransformableViews.ToList());
            Container.Bind<IDataProvider<LevelDataProgress>>().To<DataProvider<LevelDataProgress>>().AsSingle().WithArguments(SavableConstants.CurrentLevelProgressFileName);

            Container.BindInterfacesAndSelfTo<MouseService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BirdsSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<BirdRespawnService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BirdsHealthContainer>().AsSingle();
            Container.BindInterfacesAndSelfTo<MechanicsByLevelActivator>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelDataChooser>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelLoadService>().AsSingle();
            Container.BindInterfacesAndSelfTo<UpdateServiceForNewLevel>().AsSingle();
            Container.BindInterfacesAndSelfTo<EffectActivator>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallsMovementSystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridDataService>().AsSingle().WithArguments(_header);
            Container.BindInterfacesAndSelfTo<AngleCorrector>().AsSingle();
            Container.BindInterfacesTo<GridPositionResolver>().AsSingle();
            
            Container.Bind<IShapePositionChecker>().To<PlayerShapePositionChecker>().AsSingle().WithArguments(_playerShape);
            Container.Bind<PlayerCollisionService>().AsSingle().WithArguments(_playerShape).NonLazy();
            
            BindPlayerMoving();
            BindHealthPointService();
            
            Container.Bind<IWallLoader>().To<WallLoader>().AsSingle().WithArguments(_wallPrefab);
            Container.Bind<IEntityViewService>().To<EntityViewService>().AsSingle();
            Container.Bind<IServicesActivator>().To<ServiceActivator>().AsSingle();
            Container.Bind<IGetDamageService>().To<GetDamageService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallsService>().AsSingle().WhenNotInjectedInto<GameLoopState>();
            Container.BindInterfacesAndSelfTo<LevelPackProgressService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelPackInfoViewModel>().AsSingle().WithArguments(_levelPackInfoView, _levelPackBackground);
            Container.BindInterfacesAndSelfTo<EntityCollisionsService>().AsSingle();

            Container.BindInterfacesAndSelfTo<SkipLevelService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelProgressSaveService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelProgressSaveHandler>().AsSingle().NonLazy();

            StateMachineInstaller.Install(Container, _projectStateMachine);
            
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
            Container.BindInterfacesTo<ViewHealthPointService>().AsSingle().WithArguments(_healthParent as ITransformable);
            Container.BindInterfacesTo<HealthContainer>().AsSingle();
        }

        private void BindPlayerMoving()
        {
            Container
                .BindInterfacesTo<PlayerShapeMover>()
                .AsSingle()
                .WithArguments(_playerShape).
                WhenNotInjectedInto<TickableManager>();
        }
    }
}