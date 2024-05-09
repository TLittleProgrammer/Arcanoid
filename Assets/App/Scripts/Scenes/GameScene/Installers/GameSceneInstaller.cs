using App.Scripts.Scenes.GameScene.Camera;
using App.Scripts.Scenes.GameScene.Entities;
using App.Scripts.Scenes.GameScene.Grid;
using App.Scripts.Scenes.GameScene.Input;
using App.Scripts.Scenes.GameScene.LevelManager;
using App.Scripts.Scenes.GameScene.Levels;
using App.Scripts.Scenes.GameScene.Levels.Load;
using App.Scripts.Scenes.GameScene.PlayerShape;
using App.Scripts.Scenes.GameScene.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.ScreenInfo;
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
        [SerializeField] private GameObject _blockPrefab;

        //[Inject] private FactorySettings _factorySettings;

        public void Initialize()
        {
            Container.Resolve<IGridPositionResolver>().AsyncInitialize(JsonConvert.DeserializeObject<LevelData>(_levelData.text));
            
            LoadLevel();
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSceneInstaller>().FromInstance(this).AsSingle();

            BindTimeProvider();
            BindCameraService();
            BindScreenInfoProvider();
            BindInput();
            BindGridPositionResolver();
            BindLoadLevelManager();
            BindPlayerMoving();
            BindLevelLoader();
            //BindFactories();
        }

        private void BindFactories()
        {
            Container.BindFactory<Object, EntityView, EntityView.Factory>().FromFactory<PrefabFactory<EntityView>>();
        }

        private void BindLevelLoader()
        {
            Container.Bind<ILevelLoader>().To<LevelLoader>().AsSingle().WithArguments(_blockPrefab);
        }

        private void BindTimeProvider()
        {
            Container.Bind<ITimeProvider>().To<TimeProvider>().AsSingle();
        }

        private void BindPlayerMoving()
        {
            ShapeMoverSettings shapeMoverSettings = new();
            shapeMoverSettings.Speed = 5f;

            Container.Bind<IPlayerPositionChecker>().To<PlayerPositionChecker>().AsSingle().WithArguments(_playerShape);
            Container.BindInterfacesAndSelfTo<PlayerShapeMover>().AsSingle().WithArguments(_playerShape, shapeMoverSettings);
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