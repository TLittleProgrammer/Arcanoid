using App.Scripts.Scenes.GameScene.Features.Restart;

namespace App.Scripts.Scenes.GameScene.Command
{
    public class RestartCommand : IRestartCommand
    {
        private readonly IRestartService _restartService;

        public RestartCommand(IRestartService restartService)
        {
            _restartService = restartService;
        }
        
        public void Execute()
        {
            _restartService.TryRestartLevel();
        }
    }
}