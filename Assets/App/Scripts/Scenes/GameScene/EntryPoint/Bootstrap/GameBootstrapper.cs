using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.States.Bootstrap;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.Bootstrap
{
    public class GameBootstrapper : IInitializable
    {
        private readonly IStateMachine _stateMachine;

        public GameBootstrapper(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public void Initialize()
        {
            _stateMachine.Enter<BootstrapState>();
        }
    }
}