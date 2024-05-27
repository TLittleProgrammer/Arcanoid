using System.Collections.Generic;
using System.Linq;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.LevelView;
using App.Scripts.Scenes.GameScene.States;
using Zenject;
using Zenject.ReflectionBaking.Mono.CompilerServices.SymbolWriter;

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

            Container.Bind<IStateMachine>().To<StateMachine>().AsSingle();
            
            Container.Bind(typeof(IExitableState), typeof(ITickable), typeof(GameLoopState)).To<GameLoopState>().AsSingle().WithArguments(_gameLoopTickables, stateMachine);
            Container.Bind(typeof(IExitableState), typeof(WinState)).To<WinState>().AsSingle().WithArguments(stateMachine);
            Container.Bind(typeof(IExitableState), typeof(LoadSceneFromMainMenuState)).To<LoadSceneFromMainMenuState>().AsSingle().WithArguments(_projectStateMachine);
            Container.Bind(typeof(IExitableState), typeof(RestartState)).To<RestartState>().AsSingle().WithArguments(_restartables, stateMachine);
            Container.Bind(typeof(IExitableState), typeof(LoadNextLevelState)).To<LoadNextLevelState>().AsSingle().WithArguments(_levelPackInfoView, _restartablesForLoadNewLevel, stateMachine);
            Container.Bind(typeof(IExitableState), typeof(MenuPopupState)).To<MenuPopupState>().AsSingle();
            Container.Bind(typeof(IExitableState), typeof(LooseState)).To<LooseState>().AsSingle();
            
            Container.Bind<List<IExitableState>>().FromMethod(ctx => ctx.Container.ResolveAll<IExitableState>().ToList()).AsSingle();

            Container.Rebind<IStateMachine>().FromMethod(ctx =>
            {
                var services = ctx.Container.Resolve<List<IExitableState>>();
                return new StateMachine(services);
            }).AsSingle();
        }
    }
}