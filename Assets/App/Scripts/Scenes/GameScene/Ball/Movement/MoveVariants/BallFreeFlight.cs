using System.Runtime.InteropServices;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Settings;
using App.Scripts.Scenes.GameScene.Time;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public class BallFreeFlight : IBallFreeFlightMover
    {
        private readonly BallFlyingSettings _settings;
        private readonly ITimeProvider _timeProvider;
        private readonly IPositionChecker _positionChecker;
        private readonly ITransformable _ballTransformable;
        
        private Vector3 _direction;

        public BallFreeFlight(
            ITransformable ballTransformable,
            BallFlyingSettings settings,
            ITimeProvider timeProvider,
            [Inject(Id = "BallPositionChecker")] IPositionChecker positionChecker)
        {
            _ballTransformable = ballTransformable;
            _settings = settings;
            _timeProvider = timeProvider;
            _positionChecker = positionChecker;
        }


        public async UniTask AsyncInitialize(Vector2 param)
        {
            _direction = param.normalized;

            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            Vector2 targetPosition = _ballTransformable.Position + _direction * _settings.Speed * _timeProvider.DeltaTime;
            
            if (_positionChecker.CanChangePositionTo(targetPosition))
            {
                _ballTransformable.Position = targetPosition;
            }
        }
    }
}