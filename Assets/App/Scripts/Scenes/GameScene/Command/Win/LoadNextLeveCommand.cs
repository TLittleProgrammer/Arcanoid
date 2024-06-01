using App.Scripts.External.GameStateMachine;
using App.Scripts.General.Constants;
using App.Scripts.General.LevelPackInfoService;
using App.Scripts.Scenes.GameScene.States;

namespace App.Scripts.Scenes.GameScene.Command.Win
{
    public sealed class LoadNextLeveCommand : ILoadNextLevelCommand
    {
        private readonly ILevelPackInfoService _levelPackInfoService;
        private readonly IStateMachine _stateMachine;

        public LoadNextLeveCommand(ILevelPackInfoService levelPackInfoService, IStateMachine stateMachine)
        {
            _levelPackInfoService = levelPackInfoService;
            _stateMachine = stateMachine;
        }
        
        public void Execute()
        {
            if (_levelPackInfoService.NeedLoadNextPackOrLevel())
            {
                _stateMachine.Enter<LoadNextLevelState>();
            }
            else
            {
                _stateMachine.Enter<LoadSceneFromMainMenuState, string>(SceneNaming.MainMenu);
            }
        }
    }
}