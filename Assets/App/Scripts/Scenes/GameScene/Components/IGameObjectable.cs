using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Components
{
    public interface IGameObjectable : IComponent
    {
        GameObject GameObject { get; }
    }
}