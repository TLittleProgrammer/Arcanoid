using App.Scripts.External.Components;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Components
{
    public interface ISpriteRenderable : IComponent
    {
        SpriteRenderer SpriteRenderer { get; }
    }
}