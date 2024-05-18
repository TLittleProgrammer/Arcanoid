using App.Scripts.External.GameStateMachine;
using App.Scripts.General.States;

namespace App.Scripts.Scenes.GameScene.States
{
    public class LoadSceneFromMainMenuState : IState<string>
    {
        private readonly IStateMachine _projectStateMachine;

        public LoadSceneFromMainMenuState(IStateMachine projectStateMachine)
        {
            _projectStateMachine = projectStateMachine;
        }
        
        public void Enter(string sceneName)
        {
            _projectStateMachine.Enter<LoadingSceneState, string, bool>(sceneName, false);
        }

        public void Exit()
        {
        }
    }
}