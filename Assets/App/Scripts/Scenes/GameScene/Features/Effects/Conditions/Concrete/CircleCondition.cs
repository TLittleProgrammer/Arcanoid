using App.Scripts.Scenes.GameScene.Features.Entities.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects.Conditions
{
    public sealed class CircleCondition : ICondition
    {
        public bool Execute(IEntityView entityView, Collision2D collision)
        {
            return true;
        }
    }
}