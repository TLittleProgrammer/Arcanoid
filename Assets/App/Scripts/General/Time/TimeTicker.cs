using System;
using Zenject;

namespace App.Scripts.General.Time
{
    public sealed class TimeTicker : ITimeTicker, ITickable
    {
        public event Action SecondsTicked;

        private float _currentSeconds;
        
        public void Tick()
        {
            if (_currentSeconds >= 1f)
            {
                _currentSeconds = 0f;
                SecondsTicked?.Invoke();
            }

            _currentSeconds += UnityEngine.Time.deltaTime;
        }
    }
}