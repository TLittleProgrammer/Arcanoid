using App.Scripts.Scenes.GameScene.Ball.Movement;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Ball
{
    public sealed class BallSpeedUpdater : IBallSpeedUpdater
    {
        private IBallMovementService _ballMovementService;
        
        public async UniTask AsyncInitialize(IBallMovementService param)
        {
            _ballMovementService = param;

            await UniTask.CompletedTask;
        }

        public void UpdateSpeed()
        {
            //TODO перенести в настройки
            _ballMovementService.UpdateSpeed(0.15f);
        }
    }
}