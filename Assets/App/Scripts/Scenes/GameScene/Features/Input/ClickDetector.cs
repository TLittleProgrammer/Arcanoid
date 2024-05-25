using System;

namespace App.Scripts.Scenes.GameScene.Features.Input
{
    public sealed class ClickDetector : IClickDetector
    {
        public event Action MouseDowned;
        public event Action MouseUp;
        
        public void Tick()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                MouseDowned?.Invoke();
            }
            else
            {
                if (UnityEngine.Input.GetMouseButtonUp(0))
                {
                    MouseUp?.Invoke();
                }
            }
        }
    }
}