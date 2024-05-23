using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Input
{
    public interface IInputService : ITickable
    {
        bool UserClickDown { get; }
        Vector2 CurrentMousePosition { get; }
    }
}