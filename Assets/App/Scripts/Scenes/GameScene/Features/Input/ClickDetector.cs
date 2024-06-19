using System;

namespace App.Scripts.Scenes.GameScene.Features.Input
{
    public sealed class ClickDetector : IClickDetector
    {
        public event Action MouseDowned;
        public event Action MouseUp;

        private bool _previousIsDowned;
        
        public void Tick()
        {
            if (UnityEngine.Input.GetMouseButton(0))
            {
                if (!_previousIsDowned)
                {
                    _previousIsDowned = true;
                    MouseDowned?.Invoke();
                }
            }
            else
            {
                if (_previousIsDowned)
                {
                    MouseUp?.Invoke();
                    _previousIsDowned = false;
                }
            }
        }
    }
}