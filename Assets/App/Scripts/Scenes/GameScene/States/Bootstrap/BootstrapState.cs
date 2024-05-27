﻿using App.Scripts.External.GameStateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapState : IState
    {
        private readonly IStateMachine _stateMachine;

        public BootstrapState(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public async UniTask Enter()
        {
            _stateMachine.Enter<BootstrapServiceActivatorState>();
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}