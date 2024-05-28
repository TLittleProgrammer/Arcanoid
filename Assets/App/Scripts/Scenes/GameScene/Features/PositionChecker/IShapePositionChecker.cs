using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.PositionChecker
{
    public interface IShapePositionChecker
    {
        float MinX { get; }
        float MaxX { get; }
        
        void ChangeShapeScale();
        bool CanChangePositionTo(Vector2 targetPosition);
    }
}