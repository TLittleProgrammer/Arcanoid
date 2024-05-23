using System;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Input
{
    public interface IClickDetector : ITickable
    {
        event Action MouseDowned;
        event Action MouseUp;
    }
}