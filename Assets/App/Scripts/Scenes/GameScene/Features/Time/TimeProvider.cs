using System;

namespace App.Scripts.Scenes.GameScene.Features.Time
{
    public sealed class TimeProvider : ITimeProvider
    {
        private float _timeScale;
        public event Action TimeScaleChanged;

        public TimeProvider()
        {
            TimeScale = 1f;
        }

        public float DeltaTime => UnityEngine.Time.unscaledDeltaTime * TimeScale;
        
        public float TimeScale
        {
            get => _timeScale;
            set
            {
                _timeScale = value;
                TimeScaleChanged?.Invoke();
            }
        }

        public void Restart()
        {
            TimeScale = 1f;
        }
    }
}