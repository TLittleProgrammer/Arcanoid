using App.Scripts.Scenes.GameScene.Camera;
using App.Scripts.Scenes.GameScene.Grid;
using App.Scripts.Scenes.GameScene.Input;
using App.Scripts.Scenes.GameScene.LevelManager;
using App.Scripts.Scenes.GameScene.Levels;
using App.Scripts.Scenes.GameScene.PlayerShape;
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
        [SerializeField] private PlayerView _shapeTransformable;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSceneInstaller>().FromInstance(this).AsSingle();

            BindTimeProvider();
            BindScreenInfoProvider();
            BindCameraService();
            BindInput();
            BindGridPositionResolver();
            BindLoadLevelManager();
            BindPlayerMover();
        }

        private void BindTimeProvider()
        {
            Container.Bind<ITimeProvider>().To<TimeProvider>().AsSingle();
        }

        private void BindPlayerMover()
        {
            ShapeMoverSettings shapeMoverSettings = new();
            shapeMoverSettings.Speed = 5f;
            
            Container.BindInterfacesAndSelfTo<PlayerShapeMover>().AsSingle().WithArguments(_shapeTransformable, shapeMoverSettings);
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<ClickDetector>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
        }

        public void Initialize()
        {
            Container.Resolve<IGridPositionResolver>().AsyncInitialize(JsonConvert.DeserializeObject<LevelData>(_levelData.text));
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