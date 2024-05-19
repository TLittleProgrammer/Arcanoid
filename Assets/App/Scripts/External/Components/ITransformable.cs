using UnityEngine;

namespace App.Scripts.External.Components
{
    public interface ITransformable : IComponent
    {
        Transform Transform { get; }
    }
}