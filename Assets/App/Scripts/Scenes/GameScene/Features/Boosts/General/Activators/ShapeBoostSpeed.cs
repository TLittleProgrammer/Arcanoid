using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class ShapeBoostSpeed : IConcreteBoostActivator
    {
        private readonly IPlayerShapeMover _playerShapeMover;
        private const float DefaultSpeedMultiplier = 1f; 

        public ShapeBoostSpeed(IPlayerShapeMover playerShapeMover)
        {
            _playerShapeMover = playerShapeMover;
        }
        
        public bool IsTimeableBoost => true;

        public void Activate(IBoostDataProvider boostDataProvider)
        {
            FloatBoostData floatBoostData = (FloatBoostData)boostDataProvider;
            
            _playerShapeMover.ChangeSpeed(floatBoostData.Value);
        }

        public void Deactivate()
        {
            _playerShapeMover.ChangeSpeed(DefaultSpeedMultiplier);
        }
    }
}