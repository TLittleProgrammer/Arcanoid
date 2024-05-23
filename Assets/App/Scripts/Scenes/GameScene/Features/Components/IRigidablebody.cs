using System;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Components
{
    public interface IRigidablebody : IPositionable
    {
        event Action<Collider2D> Collidered;
        Rigidbody2D Rigidbody2D { get; }
    }
}