using App.Scripts.Scenes.GameScene.Features.Ball.Movement;
using App.Scripts.Scenes.GameScene.Features.Settings;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Features.Ball
{
    public sealed class BallSpeedUpdater : IBallSpeedUpdater
    {
        private readonly BallFlyingSettings _ballFlyingSettings;
        private IBallMovementService _ballMovementService;

        public BallSpeedUpdater(BallFlyingSettings ballFlyingSettings)
        {
            _ballFlyingSettings = ballFlyingSettings;
        }
        
        public async UniTask AsyncInitialize(IBallMovementService param)
        {
            _ballMovementService = param;

            await UniTask.CompletedTask;
        }

        public void UpdateSpeed()
        {
            _ballMovementService.UpdateSpeed(_ballFlyingSettings.AddSpeedAfterBlockDestroying);
        }
    }
}