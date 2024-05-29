using System;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Components
{
    public interface IRigidablebody : IPositionable
    {
        event Action<BallView, Collider2D> Collidered;
        Rigidbody2D Rigidbody2D { get; }
    }
}