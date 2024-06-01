using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Constants;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.Walls;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Animations;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapInitializeOtherServicesState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IWallLoader _wallLoader;
        private readonly IPopupProvider _popupProvider;
        private readonly IShowLevelAnimation _showLevelAnimation;
        private readonly ILevelPackInfoService _levelPackInfoService;

        public BootstrapInitializeOtherServicesState(
            IStateMachine stateMachine,
            IWallLoader wallLoader,
            IPopupProvider popupProvider,
            IShowLevelAnimation showLevelAnimation,
            ILevelPackInfoService levelPackInfoService)
        {
            _stateMachine = stateMachine;
            _wallLoader = wallLoader;
            _popupProvider = popupProvider;
            _showLevelAnimation = showLevelAnimation;
            _levelPackInfoService = levelPackInfoService;
        }
        
        public async UniTask Enter()
        {
            await _wallLoader.AsyncInitialize();
            await _popupProvider.AsyncInitialize(Pathes.PathToPopups);

            if (!_levelPackInfoService.NeedContinueLevel)
            {
                await _showLevelAnimation.Show();
            }

            _stateMachine.Enter<GameLoopState>().Forget();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}