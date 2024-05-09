using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Components
{
    public interface ITransformable : IComponent
    {
        Vector3 Position { get; set; }
    }
}