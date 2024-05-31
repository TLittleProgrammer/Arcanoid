using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Constants;
using App.Scripts.Scenes.GameScene.States;

namespace App.Scripts.Scenes.GameScene.Command
{
    public sealed class BackCommand : IBackCommand
    {
        private readonly IStateMachine _stateMachine;
        
        public BackCommand(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public void Execute()
        {
            _stateMachine.Enter<LoadSceneFromMainMenuState, string>(SceneNaming.MainMenu);
        }
    }
}