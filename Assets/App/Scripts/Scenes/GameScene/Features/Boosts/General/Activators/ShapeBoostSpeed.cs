using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.EntityDestroyer;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Settings;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class ShapeBoostSpeed : IConcreteBoostActivator
    {
        private IPlayerShapeMover _playerShapeMover;
        private const float _defaultSpeedMultiplier = 1f; 
        
        public float NewSpeed;

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
            _playerShapeMover.ChangeSpeed(_defaultSpeedMultiplier);
        }
    }
}