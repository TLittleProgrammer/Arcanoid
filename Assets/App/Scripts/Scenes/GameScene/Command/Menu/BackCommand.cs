using System.Collections.Generic;
using App.Scripts.External.GameStateMachine;
using App.Scripts.External.Initialization;
using App.Scripts.External.UserData;
using App.Scripts.General.Constants;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.States;

namespace App.Scripts.Scenes.GameScene.Command.Menu
{
    public sealed class BackCommand : IBackCommand
    {
        private readonly IStateMachine _stateMachine;
        private readonly IDataProvider<LevelDataProgress> _levelDataProgress;
        private readonly List<IDisposable> _disposables;

        public BackCommand(
            IStateMachine stateMachine,
            IDataProvider<LevelDataProgress> levelDataProgress,
            List<IDisposable> disposables)
        {
            _stateMachine = stateMachine;
            _levelDataProgress = levelDataProgress;
            _disposables = disposables;
        }
        
        public void Execute()
        {
            _levelDataProgress.Delete();

            DisposeAll();
            
            _stateMachine.Enter<LoadSceneFromMainMenuState, string>(SceneNaming.MainMenu);
        }

        private void DisposeAll()
        {
            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}