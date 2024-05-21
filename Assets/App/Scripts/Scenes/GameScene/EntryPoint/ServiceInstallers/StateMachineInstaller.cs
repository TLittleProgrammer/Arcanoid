using System.Collections.Generic;
using App.Scripts.External.Components;
using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.LevelView;
using App.Scripts.Scenes.GameScene.States;
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

            Container.BindInterfacesAndSelfTo<GameLoopState>().AsSingle().WithArguments(_gameLoopTickables, stateMachine);
            Container.Bind<WinState>().AsSingle().WithArguments(stateMachine);
            Container.Bind<LoadSceneFromMainMenuState>().AsSingle().WithArguments(_projectStateMachine);
            Container.Bind<RestartState>().AsSingle().WithArguments(_restartables, stateMachine);
            Container.Bind<LoadNextLevelState>().AsSingle().WithArguments(_levelPackInfoView, _restartablesForLoadNewLevel, stateMachine);
            Container.Bind<PopupState>().AsSingle();
            Container.Bind<LooseState>().AsSingle();

            Container
                .Bind<IStateMachine>()
                .FromInstance(stateMachine)
                .AsSingle();
        }
    }
}