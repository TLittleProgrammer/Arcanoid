using App.Scripts.External.GameStateMachine;
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

        public BackCommand(IStateMachine stateMachine, IDataProvider<LevelDataProgress> levelDataProgress)
        {
            _stateMachine = stateMachine;
            _levelDataProgress = levelDataProgress;
        }
        
        public void Execute()
        {
            _levelDataProgress.Delete();
            _stateMachine.Enter<LoadSceneFromMainMenuState, string>(SceneNaming.MainMenu);
        }
    }
}