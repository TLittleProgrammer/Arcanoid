using App.Scripts.Scenes.GameScene.Features.Entities.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects.Conditions.Concrete
{
    public sealed class CircleCondition : ICondition
    {
        public bool Execute(IEntityView entityView, Collider2D collider2D)
        {
            return true;
        }
    }
}