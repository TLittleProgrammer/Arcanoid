using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Components
{
    public interface IBoxColliderable2D : IComponent
    {
        BoxCollider2D BoxCollider2D { get; }
    }
}