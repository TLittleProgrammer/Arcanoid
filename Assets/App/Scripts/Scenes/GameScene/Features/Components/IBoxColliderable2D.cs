using App.Scripts.External.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Components
{
    public interface IBoxColliderable2D : IComponent
    {
        BoxCollider2D BoxCollider2D { get; }
    }
}