using App.Scripts.External.Initialization;
using App.Scripts.Scenes.GameScene.Features.Components;
using UnityEngine;
using Zenject;

namespace App.Scripts.Scenes.GameScene.Features.Entities.Bird.Interfaces
{
    public interface IBirdMovement : ITickable, IActivable, IAsyncInitializable<Vector2>
    {
        
    }
}