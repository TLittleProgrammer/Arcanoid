using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.LevelView;
using App.Scripts.Scenes.GameScene.States;
using App.Scripts.Scenes.GameScene.States.Bootstrap;
using Zenject;

namespace App.Scripts.Scenes.GameScene.EntryPoint.ServiceInstallers
{
    public class StateMachineInstaller : Installer<List<ITickable>, IStateMachine, List<IRestartable>, LevelPackInfoView, List<IRestartable>, StateMachineInstaller>
    {
        private readonly List<ITickable> _gameLoopTickables;
        private readonly IStateMachine _projectStateMachine;
        private readonly List<IRestartable> _restartables;
        private readonly LevelPackInfoView _levelPackInfoView;
        private readonly List<IRestartable> _restartablesForLoadNewLevel;

        public StateMachineInstaller(
            List<ITickable> gameLoopTickables,
            IStateMachine projectStateMachine,
            List<IRestartable> restartables,
            LevelPackInfoView levelPackInfoView,
            List<IRestartable> restartablesForLoadNewLevel
        )
        {
            _gameLoopTickables = gameLoopTickables;
            _projectStateMachine = projectStateMachine;
            _restartables = restartables;
            _levelPackInfoView = levelPackInfoView;
            _restartablesForLoadNewLevel = restartablesForLoadNewLevel;
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
            Container.BindInterfacesTo<BootstrapState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<BootstrapServiceActivatorState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<BootstrapBehaviourTreeState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<BootstrapLoadLevelState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<BootstrapItemsDestroyerState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<BootstrapInitializeAllRestartablesAndTickablesListsState>().AsSingle().WithArguments(stateMachine, _restartables, _restartablesForLoadNewLevel, _gameLoopTickables);
            Container.BindInterfacesTo<BootstrapInitializeOtherServicesState>().AsSingle().WithArguments(stateMachine);
            
            Container.BindInterfacesTo<GameLoopState>().AsSingle().WithArguments(_gameLoopTickables, stateMachine);
            Container.BindInterfacesTo<WinState>().AsSingle().WithArguments(stateMachine);
            Container.BindInterfacesTo<LoadSceneFromMainMenuState>().AsSingle().WithArguments(_projectStateMachine);
            Container.BindInterfacesTo<RestartState>().AsSingle().WithArguments(_restartables, stateMachine);
            Container.BindInterfacesTo<LoadNextLevelState>().AsSingle().WithArguments(_levelPackInfoView, _restartablesForLoadNewLevel, stateMachine);
            Container.BindInterfacesTo<MenuPopupState>().AsSingle();
            Container.BindInterfacesTo<LooseState>().AsSingle();
            
            Container.Bind<List<IExitableState>>().FromMethod(ctx => ctx.Container.ResolveAll<IExitableState>().ToList()).AsSingle();

        }
    }
}