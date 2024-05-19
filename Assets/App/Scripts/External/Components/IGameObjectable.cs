using UnityEngine;

namespace App.Scripts.External.Components
{
    public interface IGameObjectable : IComponent
    {
        GameObject GameObject { get; }
    }
}