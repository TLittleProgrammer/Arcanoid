using App.Scripts.External.Extensions.ZenjectExtensions;
using App.Scripts.Scenes.GameScene.Ball;
using App.Scripts.Scenes.GameScene.Ball.Movement;
using App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Camera;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Factories.EntityFactory;
using App.Scripts.Scenes.GameScene.Grid;
using App.Scripts.Scenes.GameScene.Input;
using App.Scripts.Scenes.GameScene.LevelManager;
using App.Scripts.Scenes.GameScene.Levels;
using App.Scripts.Scenes.GameScene.Levels.Load;
using App.Scripts.Scenes.GameScene.PlayerShape;
using App.Scripts.Scenes.GameScene.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Pools;
using App.Scripts.Scenes.GameScene.PositionChecker;
using App.Scripts.Scenes.GameScene.ScreenInfo;
using App.Scripts.Scenes.GameScene.Settings;
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

        [Inject] private FactorySettings _factorySettings;

        public void Initialize()
        {
            Container.Resolve<IGridPositionResolver>().AsyncInitialize(JsonConvert.DeserializeObject<LevelData>(_levelData.text));

            LoadLevel();
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSceneInstaller>().FromInstance(this).AsSingle();

            BindPools();
            BindPoolContainer();
            BindFactories();
            BindTimeProvider();
            BindCameraService();
            BindScreenInfoProvider();
            BindInput();
            BindGridPositionResolver();
            BindLoadLevelManager();
            BindPositionCheckers();
            BindPlayerMoving();
            BindLevelLoader();
            BindBallMovers();
            BindBallMovement();
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

            
            Container.BindInterfacesAndSelfTo<PlayerShapeMover>().AsSingle().WithArguments(_playerShape, shapeMoverSettings);
        }

        private void BindBallMovement()
        {
            Container.BindInterfacesAndSelfTo<BallMovementService>().AsSingle().WithArguments(_ballView);
        }

        private void BindFactories()
        {
            Container.BindFactory<string, IEntityView, IEntityView.Factory>().FromFactory<EntityFactory>();
        }

        private void BindPoolContainer()
        {
            Container.Bind<IPoolContainer>().To<PoolContainer>().AsSingle();
        }

        private void BindPools()
        {
            Container.BindPool<EntityView, EntityView.Pool>(_factorySettings.InitialSize, (EntityView)_factorySettings.EntityView, _factorySettings.ParentName);
        }

        private void BindLevelLoader()
        {
            Container.Bind<ILevelLoader>().To<LevelLoader>().AsSingle();
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

        private void LoadLevel()
        {
            if (_levelData is not null)
            {
                Container.Resolve<ILevelLoader>().LoadLevel(JsonConvert.DeserializeObject<LevelData>(_levelData.text));
            }
        }

        private void BindGridPositionResolver()
        {
            Container.Bind<IGridPositionResolver>().To<GridPositionResolver>().AsSingle().WithArguments(_header);
        }

        private void BindCameraService()
        {
            Container.Bind<ICameraService>().To<CameraService>().AsSingle().WithArguments(_camera);
        }

        private void BindLoadLevelManager()
        {
            Container.Bind<ILevelWorldBuilder>().To<LevelWorldBuilder>().AsSingle();
        }

        private void BindScreenInfoProvider()
        {
            Container.Bind<IScreenInfoProvider>().To<ScreenInfoProvider>().AsSingle();
        }
    }
}