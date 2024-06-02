using App.Scripts.External.UserData;
using App.Scripts.Scenes.GameScene.Command.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Levels.SavedLevelProgress;
using App.Scripts.Scenes.GameScene.Features.Restart;

namespace App.Scripts.Scenes.GameScene.Command
{
    public class RestartCommand : IRestartCommand
    {
        private readonly IRestartService _restartService;
        private readonly IDataProvider<LevelDataProgress> _levelDataProgress;

        public RestartCommand(IRestartService restartService, IDataProvider<LevelDataProgress> levelDataProgress)
        {
            _restartService = restartService;
            _levelDataProgress = levelDataProgress;
        }
        
        public void Execute()
        {
            _levelDataProgress.Delete();
            _restartService.TryRestartLevel();
        }
    }
}