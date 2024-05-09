using App.Scripts.Scenes.GameScene.Interfaces;
using App.Scripts.Scenes.GameScene.Settings;
using App.Scripts.Scenes.GameScene.Time;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Movement.MoveVariants
{
    public class BallFreeFlight : IBallMover
    {
        private readonly BallFlyingSettings _settings;
        private readonly ITimeProvider _timeProvider;
        private readonly ITransformable _ballTransformable;
        
        private Vector3 _direction;

        public BallFreeFlight(
            ITransformable ballTransformable,
            Vector2 direction,
            BallFlyingSettings settings,
            ITimeProvider timeProvider)
        {
            _ballTransformable = ballTransformable;
            _direction = direction.normalized;
            _settings = settings;
            _timeProvider = timeProvider;
        }
        
        public void Tick()
        {
            _ballTransformable.Position += _direction * _settings.Speed * _timeProvider.DeltaTime;
        }
    }
}