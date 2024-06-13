using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects.Conditions.Concrete
{
    public sealed class ExplosionByFireballCondition : ICondition
    {
        public bool Execute(IEntityView entityView, Collider2D collider2D)
        {
            if (collider2D.TryGetComponent(out BallView ballView))
            {
                return ballView.RedBall.activeSelf;
            }

            return false;
        }
    }
}