using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Levels.LevelPackInfoService;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapEntityDestroyerState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IEntityDestroyable _entityDestroyable;
        private readonly ILevelPackInfoService _levelPackInfoService;
        private Dictionary<string,IBlockDestroyService> _destroyServices;

        public BootstrapEntityDestroyerState(
            IStateMachine stateMachine,
            IEntityDestroyable entityDestroyable,
            ILevelPackInfoService levelPackInfoService,
            Dictionary<string,IBlockDestroyService> destroyServices
        )
        {
            _stateMachine = stateMachine;
            _entityDestroyable = entityDestroyable;
            _levelPackInfoService = levelPackInfoService;
            _destroyServices = destroyServices;
        }
        
        public UniTask Enter()
        {
            _entityDestroyable.AsyncInitialize(_destroyServices);
            if (_levelPackInfoService.NeedContinueLevel)
            {
                _stateMachine.Enter<BootstrapContinueLoadLevelState>().Forget();
            }
            else
            {
                _stateMachine.Enter<BootstrapLoadLevelState>().Forget();
            }
            
            return UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}