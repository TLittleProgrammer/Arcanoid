﻿using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer.DestroyServices;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapEntityDestroyerState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IEntityDestroyable _entityDestroyable;
        private readonly List<IBlockDestroyService> _entityDestroyers;

        public BootstrapEntityDestroyerState(
            IStateMachine stateMachine,
            IEntityDestroyable entityDestroyable,
            List<IBlockDestroyService> entityDestroyers
        )
        {
            _stateMachine = stateMachine;
            _entityDestroyable = entityDestroyable;
            _entityDestroyers = entityDestroyers;
        }
        
        public async UniTask Enter()
        {
            InitializeItemsDestroyable();

            _stateMachine.Enter<BootstrapLoadLevelState>();
            
            await UniTask.CompletedTask;
        }

        private void InitializeItemsDestroyable()
        {
            List<DestroyServiceData> datas = new();

            foreach (IBlockDestroyService destroyer in _entityDestroyers)
            {
                foreach (BoostTypeId boost in destroyer.ProccessingBoostTypes)
                {
                    datas.Add(BuildDestroyDataService(boost, destroyer));
                }
            }
            
            _entityDestroyable.AsyncInitialize(datas);
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
        
        private DestroyServiceData BuildDestroyDataService(BoostTypeId boostTypeId, IBlockDestroyService blockDestroyService)
        {
            return new()
            {
                BoostTypeId = boostTypeId,
                BlockDestroyService = blockDestroyService
            };
        }
    }
}