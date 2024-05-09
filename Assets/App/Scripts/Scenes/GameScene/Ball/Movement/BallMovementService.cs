using App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Input;
using App.Scripts.Scenes.GameScene.Interfaces;
using Cysharp.Threading.Tasks;

namespace App.Scripts.Scenes.GameScene.Ball.Movement
{
    public sealed class BallMovementService : IBallMovementService
    {
        private readonly ITransformable _ballTransformable;
        private readonly ITransformable _targetTransformable;
        private readonly IClickDetector _clickDetector;
        
        private IBallMover _ballMover;
        
        public BallMovementService(
            ITransformable ballTransformable,
            ITransformable targetTransformable,
            IClickDetector clickDetector
        )
        {
            _ballTransformable = ballTransformable;
            _targetTransformable = targetTransformable;
            _clickDetector = clickDetector;
        }

        public async UniTask AsyncInitialize()
        {
            _ballMover = new BallFollowMover(_ballTransformable, _targetTransformable);

            _clickDetector.MouseUp += OnMouseUp;

            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            if (_ballMover is null)
                return;
            
            _ballMover.Tick();
        }

        private void OnMouseUp()
        {
            _clickDetector.MouseUp -= OnMouseUp;
            _ballMover = new BallFreeFlight();
        }
    }
}