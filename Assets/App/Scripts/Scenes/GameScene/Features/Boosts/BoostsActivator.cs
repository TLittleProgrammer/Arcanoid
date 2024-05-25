using System.Collections.Generic;
using App.Scripts.Scenes.GameScene.Features.Boosts.Helpers;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Levels.ItemsDestroyer.Helpers;

namespace App.Scripts.Scenes.GameScene.Features.Boosts
{
    public class BoostsActivator : IBoostsActivator
    {
        private readonly SimpleDestroyService _simpleDestroyService;
        private readonly IBoostContainer _boostContainer;
        private readonly Dictionary<BoostTypeId, IConcreteBoostActivator> _activators;

        private float _initialBallSpeed;

        public BoostsActivator(
            SimpleDestroyService simpleDestroyService,
            IBoostContainer boostContainer,
            BallMoverBoostActivator ballMoverBoostActivator,
            PlayerShapeBoostSize playerShapeBoostSize,
            ShapeBoostSpeed shapeBoostSpeed,
            HealthAndDeathBoost healthAndDeathBoost)
        {
            _simpleDestroyService = simpleDestroyService;
            _boostContainer = boostContainer;

            _activators = new()
            {
                [BoostTypeId.BallAcceleration] = ballMoverBoostActivator,
                [BoostTypeId.BallSlowdown] = ballMoverBoostActivator,
                [BoostTypeId.PlayerShapeAddSize] = playerShapeBoostSize,
                [BoostTypeId.PlayerShapeMinusSize] = playerShapeBoostSize,
                [BoostTypeId.PlayerShapeAddSpeed] = shapeBoostSpeed,
                [BoostTypeId.PlayerShapeMinusSpeed] = shapeBoostSpeed,
                [BoostTypeId.AddHealth] = healthAndDeathBoost,
                [BoostTypeId.MinusHealth] = healthAndDeathBoost,
            };
        }

        public void Activate(BoostView view)
        {
            BoostTypeId boostTypeId = view.BoostTypeId;
            
            _activators[boostTypeId].Activate(boostTypeId);

            if (boostTypeId is not BoostTypeId.AddHealth && boostTypeId is not BoostTypeId.MinusHealth)
            {
                _boostContainer.AddActive(view.BoostTypeId);
            }
            _simpleDestroyService.Destroy(view);
        }
    }
}