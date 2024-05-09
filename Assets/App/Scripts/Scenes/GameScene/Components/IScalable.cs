using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Components
{
    public interface IScalable : IComponent
    {
        Vector3 Scale { get; set; }
    }
}