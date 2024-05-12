using App.Scripts.Scenes.GameScene.Components;
using UnityEngine;

namespace App.Scripts.General.Components
{
    public interface ITransformable : IComponent
    {
        Transform Transform { get; }
    }
}