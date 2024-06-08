using System;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects.Conditions
{
    public interface IConditionService
    {
        bool Execute(Type conditionType, IEntityView entityView, Collision2D collision2D);
    }
}