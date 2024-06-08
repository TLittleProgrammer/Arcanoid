using App.Scripts.Scenes.GameScene.Features.Boosts.General;
using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.Ball;
using App.Scripts.Scenes.GameScene.Features.Entities.View;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Effects.Conditions.Concrete
{
    public sealed class ExplosionByFireballCondition : ICondition
    {
        private readonly IBoostContainer _boostContainer;

        public ExplosionByFireballCondition(IBoostContainer boostContainer)
        {
            _boostContainer = boostContainer;
        }
        
        public bool Execute(IEntityView entityView, Collider2D collider2D)
        {
            if (collider2D.TryGetComponent(out BallView ballView))
            {
                return _boostContainer.BoostIsActive(BoostTypeId.Fireball);
            }

            return false;
        }
    }
}