using System;
using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.Scenes.GameScene.Command;
using App.Scripts.Scenes.GameScene.Command.Menu;
using App.Scripts.Scenes.GameScene.Command.Win;
using App.Scripts.Scenes.GameScene.Features.Levels.LevelView;
using App.Scripts.Scenes.GameScene.Features.Restart;
using App.Scripts.Scenes.GameScene.States;
using App.Scripts.Scenes.GameScene.States.Bootstrap;
using App.Scripts.Scenes.GameScene.States.Gameloop;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class StateMachineInstaller : Installer<IStateMachine, StateMachineInstaller>
    {
        private readonly IStateMachine _projectStateMachine;

        public StateMachineInstaller(IStateMachine projectStateMachine)
        {
            _projectStateMachine = projectStateMachine;
        }

        public override void InstallBindings()
        {
            IStateMachine stateMachine = new StateMachine();
            
            BindStates(stateMachine);
            RebindStateMachine(stateMachine);
        }

        private void RebindStateMachine(IStateMachine stateMachine)
        {
            Container.Rebind<IStateMachine>().FromMethod(ctx =>
            {
                var services = ctx.Container.Resolve<List<IExitableState>>();

                stateMachine.AsyncInitialize(services);
                return stateMachine;
            }).AsSingle();
        }

        private void BindStates(IStateMachine stateMachine)
        {
            Container.Bind<GameLoopSubscriber>().AsSingle().WithArguments(stateMachine);
            
            Container.BindInterfacesAndSelfTo<GameLoopState>().AsSingle().WhenNotInjectedInto<GameLoopState>();

            Container.BindInterfacesTo<BootstrapState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<BootstrapBehaviourTreeState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<BootstrapLoadLevelState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<BootstrapEntityDestroyerState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<BootstrapInitializeOtherServicesState>().AsSingle().WithArguments(stateMachine);
            
            Container.BindInterfacesTo<BootstrapContinueLoadLevelState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<WinState>().AsSingle();
            Container.BindInterfacesTo<LoadSceneFromMainMenuState>().AsSingle().WithArguments(_projectStateMachine);
            Container.BindInterfacesTo<RestartState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<LoadNextLevelState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<MenuPopupState>().AsSingle();
            Container.BindInterfacesTo<LooseState>().AsSingle();
        
        
            Container.BindInterfacesAndSelfTo<RestartService>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesAndSelfTo<ContinueCommand>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesAndSelfTo<LoadNextLeveCommand>().AsSingle().WithArguments(stateMachine);
        }
    }
}