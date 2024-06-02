using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.PositionChecker
{
    public interface IShapePositionChecker
    {
        float MinX { get; }
        float MaxX { get; }
        
        void ChangeShapeScale();
        bool CanChangePositionTo(Vector2 targetPosition);
    }
}