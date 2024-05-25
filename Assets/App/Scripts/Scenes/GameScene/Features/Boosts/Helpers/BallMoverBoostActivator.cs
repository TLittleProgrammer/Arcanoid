using App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants;
using App.Scripts.Scenes.GameScene.Features.Boosts.Interfaces;
using App.Scripts.Scenes.GameScene.Features.Entities;
using App.Scripts.Scenes.GameScene.Features.Settings;

namespace App.Scripts.Scenes.GameScene.Features.Boosts.Helpers
{
    public sealed class BallMoverBoostActivator : IConcreteBoostActivator
    {
        private readonly IBoostContainer _boostContainer;
        private readonly IBallFreeFlightMover _ballMover;
        private readonly BoostsSettings _boostsSettings;
        private readonly float _initialBallSpeed;

        public BallMoverBoostActivator(IBoostContainer boostContainer, IBallFreeFlightMover ballMover, BoostsSettings boostsSettings)
        {
            _boostContainer = boostContainer;
            _ballMover = ballMover;
            _boostsSettings = boostsSettings;

            _boostContainer.BoostEnded += OnBoostEnded;
        }

        public void Activate(BoostTypeId boostTypeId)
        {
            if (boostTypeId is BoostTypeId.BallAcceleration)
            {
                _ballMover.SetSpeed(_ballMover.ConstantSpeed * _boostsSettings.AcceleratePercentFromAll);
            }
            else
            {
                _ballMover.SetSpeed(_ballMover.ConstantSpeed * _boostsSettings.SlowDownPercentFromAll);
            }
        }

        private void OnBoostEnded(BoostTypeId boostType)
        {
            if (boostType is BoostTypeId.BallAcceleration or BoostTypeId.BallSlowdown)
            {
                _ballMover.SetSpeed(_ballMover.ConstantSpeed);
            }
        }
    }
}