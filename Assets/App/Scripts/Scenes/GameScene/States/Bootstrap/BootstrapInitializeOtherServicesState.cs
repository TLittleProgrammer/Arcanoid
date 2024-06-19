using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Levels.LevelPackInfoService;
using App.Scripts.Scenes.GameScene.Features.Entities.Walls;
using App.Scripts.Scenes.GameScene.Features.Levels.General.Animations;
using App.Scripts.Scenes.GameScene.States.Gameloop;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapInitializeOtherServicesState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IWallLoader _wallLoader;
        private readonly IShowLevelService _showLevelService;
        private readonly ILevelPackInfoService _levelPackInfoService;

        public BootstrapInitializeOtherServicesState(
            IStateMachine stateMachine,
            IWallLoader wallLoader,
            IShowLevelService showLevelService,
            ILevelPackInfoService levelPackInfoService)
        {
            _stateMachine = stateMachine;
            _wallLoader = wallLoader;
            _showLevelService = showLevelService;
            _levelPackInfoService = levelPackInfoService;
        }
        
        public async UniTask Enter()
        {
            await _wallLoader.AsyncInitialize();

            if (!_levelPackInfoService.NeedContinueLevel)
            {
                await UniTask.Delay(1000);
                await _showLevelService.Show();
            }

            _stateMachine.Enter<GameLoopState>().Forget();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}