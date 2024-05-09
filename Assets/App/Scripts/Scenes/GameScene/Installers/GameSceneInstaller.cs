using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Camera;
using App.Scripts.Scenes.GameScene.Grid;
using App.Scripts.Scenes.GameScene.LevelManager;
using App.Scripts.Scenes.GameScene.Levels;
using App.Scripts.Scenes.GameScene.ScreenInfo;
using Cysharp.Threading.Tasks;
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

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameSceneInstaller>().FromInstance(this).AsSingle();
            
            BindScreenInfoProvider();
            BindCameraService();
            BindGridPositionResolver();
            BindLoadLevelManager();
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