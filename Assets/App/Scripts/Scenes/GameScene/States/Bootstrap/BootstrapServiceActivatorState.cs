﻿using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Features.Boosts;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Components;
using App.Scripts.Scenes.GameScene.Features.MiniGun;
using App.Scripts.Scenes.GameScene.Features.ServiceActivator;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.States.Bootstrap
{
    public class BootstrapServiceActivatorState : IState
    {
        private readonly IStateMachine _stateMachine;
        private readonly IServicesActivator _servicesActivator;
        private readonly List<IActivable> _activables;

        public BootstrapServiceActivatorState(
            IStateMachine stateMachine,
            IServicesActivator servicesActivator,
            List<IActivable> activables)
        {
            _stateMachine = stateMachine;
            _servicesActivator = servicesActivator;
            _activables = activables;
        }
        
        public async UniTask Enter()
        {
            foreach (IActivable activable in _activables)
            {
                _servicesActivator.AddActivable(activable);
            }
            
            _stateMachine.Enter<BootstrapBehaviourTreeState>();
            
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}