using UnityEngine;

namespace App.Scripts.External.Components
{
    public interface IRectTransformable : IComponent
    {
        RectTransform RectTransform { get; }
    }
}