using App.Scripts.Scenes.GameScene.Features.Entities.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects.Conditions
{
    public interface ICondition
    {
        bool Execute(IEntityView entityView, Collision2D collision);
    }
}