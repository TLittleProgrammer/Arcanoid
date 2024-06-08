using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Entities.View
{
    public interface IEffectActivator
    {
        void ActivateEffect(IEntityView entityView, Collider2D collider2D);
    }
}