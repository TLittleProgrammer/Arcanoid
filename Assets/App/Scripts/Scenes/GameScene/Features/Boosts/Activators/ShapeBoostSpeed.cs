using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.PlayerShape.Move;
using App.Scripts.Scenes.GameScene.Features.Settings;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Helpers
{
    public sealed class ShapeBoostSpeed : IConcreteBoostActivator
    {
        private readonly IBoostContainer _boostContainer;
        private readonly IPlayerShapeMover _playerShapeMover;
        private readonly BoostsSettings _boostsSettings;

        public ShapeBoostSpeed(IBoostContainer boostContainer, IPlayerShapeMover playerShapeMover, BoostsSettings boostsSettings)
        {
            _boostContainer = boostContainer;
            _playerShapeMover = playerShapeMover;
            _boostsSettings = boostsSettings;

            boostContainer.BoostEnded += OnBoostEnded;
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

        private void OnBoostEnded(BoostTypeId boostType)
        {
            if (boostType is BoostTypeId.PlayerShapeAddSpeed or BoostTypeId.PlayerShapeMinusSpeed)
            {
                _playerShapeMover.ChangeSpeed(1f);
            }
        }
    }
}