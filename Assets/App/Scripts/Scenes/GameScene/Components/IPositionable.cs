using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Components
{
    public interface IPositionable : IComponent
    {
        Vector3 Position { get; set; }
    }
}