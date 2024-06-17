using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class ShapeBoostSpeed : IConcreteBoostActivator
    {
        private readonly IPlayerShapeMover _playerShapeMover;
        private readonly FloatBoostData _boostDataProvider;
        
        private const float DefaultSpeedMultiplier = 1f; 

        public ShapeBoostSpeed(IPlayerShapeMover playerShapeMover, FloatBoostData boostDataProvider)
        {
            _playerShapeMover = playerShapeMover;
            _boostDataProvider = boostDataProvider;
        }
        
        public bool IsTimeableBoost => true;

        public void Activate()
        {
            _playerShapeMover.ChangeSpeed(_boostDataProvider.Value);
        }

        public void Deactivate()
        {
            _playerShapeMover.ChangeSpeed(DefaultSpeedMultiplier);
        }
    }
}