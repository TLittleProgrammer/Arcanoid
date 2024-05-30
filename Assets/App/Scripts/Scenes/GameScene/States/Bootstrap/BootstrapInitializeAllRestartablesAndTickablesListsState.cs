using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Input;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapInitializeAllRestartablesAndTickablesListsState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IPlayerShapeMover _playerShapeMover;
        private readonly IClickDetector _clickDetector;
        private readonly IInputService _inputService;
        private readonly GameLoopState _gameLoopState;
        private readonly IBallsService _ballsService;

        public BootstrapInitializeAllRestartablesAndTickablesListsState(
            IStateMachine stateMachine,
            IPlayerShapeMover playerShapeMover,
            IClickDetector clickDetector,
            IInputService inputService,
            GameLoopState gameLoopState,
            IBallsService ballsService)
        {
            _stateMachine = stateMachine;
            _playerShapeMover = playerShapeMover;
            _clickDetector = clickDetector;
            _inputService = inputService;
            _gameLoopState = gameLoopState;
            _ballsService = ballsService;
        }
        
        public async UniTask Enter()
        {
            InitializeTickablesList();
            
            _stateMachine.Enter<GameLoopState>();

            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }

        private void InitializeTickablesList()
        {
            _gameLoopState.AddTickable(_playerShapeMover);
            _gameLoopState.AddTickable(_clickDetector);
            _gameLoopState.AddTickable(_inputService);
            _gameLoopState.AddTickable(_ballsService);
        }
    }
}