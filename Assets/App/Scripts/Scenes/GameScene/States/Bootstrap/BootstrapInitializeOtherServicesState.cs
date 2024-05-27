using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Constants;
using App.Scripts.General.Popup.AssetManagment;
using App.Scripts.Scenes.GameScene.Features.Ball;
using App.Scripts.Scenes.GameScene.Features.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Levels.Animations;
using App.Scripts.Scenes.GameScene.Features.Walls;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapInitializeOtherServicesState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IWallLoader _wallLoader;
        private readonly IPopupProvider _popupProvider;
        private readonly IBallSpeedUpdater _ballSpeedUpdater;
        private readonly IBallMovementService _ballMovementService;
        private readonly IShowLevelAnimation _showLevelAnimation;

        public BootstrapInitializeOtherServicesState(
            IStateMachine stateMachine,
            IWallLoader wallLoader,
            IPopupProvider popupProvider,
            IBallSpeedUpdater ballSpeedUpdater,
            IBallMovementService ballMovementService,
            IShowLevelAnimation showLevelAnimation)
        {
            _stateMachine = stateMachine;
            _wallLoader = wallLoader;
            _popupProvider = popupProvider;
            _ballSpeedUpdater = ballSpeedUpdater;
            _ballMovementService = ballMovementService;
            _showLevelAnimation = showLevelAnimation;
        }
        
        public async UniTask Enter()
        {
            await _wallLoader.AsyncInitialize();
            await _popupProvider.AsyncInitialize(Pathes.PathToPopups);
            await _ballSpeedUpdater.AsyncInitialize(_ballMovementService);
            await _showLevelAnimation.Show();

            _stateMachine.Enter<BootstrapInitializeAllRestartablesAndTickablesListsState>();
            
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}