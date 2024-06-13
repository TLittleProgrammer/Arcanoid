using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants
{
    public sealed class AngleCorrector : IAngleCorrector
    {
        private readonly float _maxSecondAngle;
        private readonly float _minSecondAngle;
        private BallFlyingSettings _settings;

        public AngleCorrector(BallFlyingSettings settings)
        {
            _settings = settings;
            _maxSecondAngle = 180f - settings.MaxAngle;
            _minSecondAngle = 180f - settings.MinAngle;
        }
        
        public float CorrectAngleByDirection(Vector2 direction)
        {
            float currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float absCurrentAngle = Mathf.Abs(currentAngle);
            
            return ChooseTargetAngle(absCurrentAngle, currentAngle);
        }

        private float ChooseTargetAngle(float absCurrentAngle, float currentAngle)
        {
            float multiplier = currentAngle > 0 ? 1f : -1f;
            
            if (absCurrentAngle < _settings.MinAngle)
            {
                return  _settings.MinAngle * multiplier;
            }

            if (absCurrentAngle > _settings.MaxAngle && _maxSecondAngle > absCurrentAngle)
            {
                return _settings.MaxAngle * multiplier;
            }

            if (absCurrentAngle > _minSecondAngle)
            {
                return _minSecondAngle * multiplier;
            }

            return currentAngle;
        }
    }
}