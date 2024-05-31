using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.UserData;
using App.Scripts.External.UserData.SaveLoad;
using App.Scripts.General.Infrastructure;
using App.Scripts.General.Levels;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Grid;
using App.Scripts.Scenes.GameScene.Features.Healthes;
using App.Scripts.Scenes.GameScene.Features.Levels.General;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelProgress;
using App.Scripts.Scenes.GameScene.Features.Levels.Loading;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapContinueLoadLevelState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly List<IInitializeByLevelProgress> _initializeByLevelProgresses;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IGridPositionResolver _gridPositionResolver;
        private readonly IUserDataContainer _userDataContainer;

        public BootstrapContinueLoadLevelState(
            IStateMachine stateMachine,
            List<IInitializeByLevelProgress> initializeByLevelProgresses,
            ISaveLoadService saveLoadService,
            IGridPositionResolver gridPositionResolver)
        {
            _stateMachine = stateMachine;
            _initializeByLevelProgresses = initializeByLevelProgresses;
            _saveLoadService = saveLoadService;
            _gridPositionResolver = gridPositionResolver;
        }
        
        public async UniTask Enter()
        {
            LevelDataProgress levelDataProgress = new();
            _saveLoadService.Load(ref levelDataProgress);
            
            await _gridPositionResolver.AsyncInitialize(levelDataProgress.LevelData);

            foreach (IInitializeByLevelProgress progress in _initializeByLevelProgresses)
            {
                progress.LoadProgress(levelDataProgress);
            }

            _stateMachine.Enter<BootstrapInitializeOtherServicesState>();
            
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}