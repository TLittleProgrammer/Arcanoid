using App.Scripts.General.Components;
using App.Scripts.Scenes.GameScene.Components;
using App.Scripts.Scenes.GameScene.Healthes;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Ball.Collision
{
    public class BallCollisionService : IBallCollisionService
    {
        private readonly IRigidablebody _ball;
        private readonly IPositionable _playerView;
        private readonly IHealthContainer _healthContainer;

        public BallCollisionService(
            IRigidablebody ball,
            IPositionable playerView,
            IHealthContainer healthContainer)
        {
            _ball = ball;
            _playerView = playerView;
            _healthContainer = healthContainer;
            _ball.Collidered += OnCollidered;
        }

        private void OnCollidered(Collider2D obj)
        {
            if (_ball.Position.y < _playerView.Position.y)
            {
                _healthContainer.UpdateHealth(-1);
            }
        }
    }
}