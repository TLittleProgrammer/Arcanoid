using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Input
{
    public interface IInputService
    {
        bool UserClickDown { get; }
        Vector2 CurrentMousePosition { get; }
    }
}