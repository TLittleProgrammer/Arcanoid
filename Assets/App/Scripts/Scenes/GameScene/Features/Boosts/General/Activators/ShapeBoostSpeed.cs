using App.Scripts.Scenes.GameScene.Features.Boosts.General.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Settings;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.General.Activators
{
    public sealed class ShapeBoostSpeed : IConcreteBoostActivator
    {
        private readonly IPlayerShapeMover _playerShapeMover;
        private readonly BoostsSettings _boostsSettings;

        public ShapeBoostSpeed(IPlayerShapeMover playerShapeMover, BoostsSettings boostsSettings)
        {
            _playerShapeMover = playerShapeMover;
            _boostsSettings = boostsSettings;
        }

        public void Activate(BoostTypeId boostTypeId)
        {
            if (boostTypeId is BoostTypeId.PlayerShapeAddSpeed)
            {
                _playerShapeMover.ChangeSpeed(_boostsSettings.AddPercentSpeed);
            }
            else
            {
                _playerShapeMover.ChangeSpeed(_boostsSettings.MinusPercentSpeed);
            }
        }

        public void Deactivate()
        {
            _playerShapeMover.ChangeSpeed(1f);
        }
    }
}