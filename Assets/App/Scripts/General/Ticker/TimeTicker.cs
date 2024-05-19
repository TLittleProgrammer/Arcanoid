using System;
using UnityEngine;
using Zenject;

namespace App.Scripts.General.Ticker
{
    public sealed class TimeTicker : ITimeTicker, ITickable
    {
        public event Action SecondsTicked;
        public event Action MinutesTicked;

        private int _seconds = 0;
        private float _deltaOffset = 0;
        
        public void Tick()
        {
            _deltaOffset += Time.deltaTime;

            if (_deltaOffset >= 1f)
            {
                _deltaOffset = 0f;
                _seconds++;
                SecondsTicked?.Invoke();

                if (_seconds >= 60)
                {
                    _seconds = 0;
                    MinutesTicked?.Invoke();
                }
            }
        }
    }
}