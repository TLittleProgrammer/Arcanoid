using App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Input;
using App.Scripts.Scenes.GameScene.Interfaces;
using App.Scripts.Scenes.GameScene.Settings;
using App.Scripts.Scenes.GameScene.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Movement
{
    public sealed class BallMovementService : IBallMovementService
    {
        private readonly ITransformable _ballTransformable;
        private readonly ITransformable _targetTransformable;
        private readonly IClickDetector _clickDetector;
        private readonly ITimeProvider _timeProvider;
        private readonly BallFlyingSettings _ballFlyingSettings;

        private IBallMover _ballMover;
        private Vector2 _previousBallPosition;
        
        public BallMovementService(
            ITransformable ballTransformable,
            ITransformable targetTransformable,
            IClickDetector clickDetector,
            ITimeProvider timeProvider,
            BallFlyingSettings ballFlyingSettings
        )
        {
            _ballTransformable = ballTransformable;
            _targetTransformable = targetTransformable;
            _clickDetector = clickDetector;
            _timeProvider = timeProvider;
            _ballFlyingSettings = ballFlyingSettings;
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

            _previousBallPosition = _ballTransformable.Position;
            _ballMover.Tick();
        }

        private void OnMouseUp()
        {
            _clickDetector.MouseUp -= OnMouseUp;

            Vector2 direction = GetDirection();
            
            _ballMover = new BallFreeFlight(_ballTransformable, direction, _ballFlyingSettings, _timeProvider);
        }

        private Vector2 GetDirection()
        {
            return _ballTransformable.Position.x > _previousBallPosition.x
                ? new Vector2(0.5f, 0.5f)
                : new Vector2(-0.5f, 0.5f);
        }
    }
}