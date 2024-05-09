using System;

namespace App.Scripts.Scenes.GameScene.Input
{
    public interface IClickDetector
    {
        event Action MouseDowned;
        event Action MouseUp;
    }
}