using App.Scripts.External.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Components
{
    public interface IPositionable : IGameObjectable
    {
        Vector3 Position { get; set; }
    }
}