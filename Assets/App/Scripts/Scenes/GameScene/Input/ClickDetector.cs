using System;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Input
{
    public sealed class ClickDetector : IClickDetector, ITickable
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