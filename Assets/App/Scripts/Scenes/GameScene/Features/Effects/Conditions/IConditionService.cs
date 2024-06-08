using System;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects.Conditions
{
    public interface IConditionService
    {
        bool Execute(Type conditionType, IEntityView entityView, Collider2D collider2D);
    }
}