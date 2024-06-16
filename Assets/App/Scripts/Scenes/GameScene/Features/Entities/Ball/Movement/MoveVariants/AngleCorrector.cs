using App.Scripts.Scenes.GameScene.Features.Settings;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Ball.Movement.MoveVariants
{
    public sealed class AngleCorrector : IAngleCorrector
    {
        private readonly float _minAngle;
        private readonly float[] _axesAngles = { 0f, 90f, 180f, -90f, -180f };

        public AngleCorrector(BallFlyingSettings settings)
        {
            _minAngle = settings.MinAngle;
        }
        
        public float CorrectAngleByDirection(Vector2 direction)
        {
            float currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            foreach (var axesAngle in _axesAngles)
            {
                float difference = Mathf.DeltaAngle(currentAngle, axesAngle);

                if (Mathf.Abs(difference) < _minAngle)
                {
                    return axesAngle + _minAngle * (difference <= 0f ? 1f : -1f);
                }
            }

            return currentAngle;
        }
    }
}