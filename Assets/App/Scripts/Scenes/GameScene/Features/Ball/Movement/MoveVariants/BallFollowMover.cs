using App.Scripts.General.Infrastructure;
using App.Scripts.Scenes.GameScene.Features.Components;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Scripts.Scenes.GameScene.Features.Ball.Movement.MoveVariants
{
    public sealed class BallFollowMover : IBallFollowMover
    {
        private readonly IPositionable _ballPositionable;
        private readonly IPositionable _targetPositionable;
        private Vector3 _initialOffset;
        private Vector3 _stickyOffset;

        public BallFollowMover(IPositionable ballPositionable, IPositionable targetPositionable)
        {
            _ballPositionable = ballPositionable;
            _targetPositionable = targetPositionable;
            _initialOffset = _stickyOffset = ballPositionable.Position - targetPositionable.Position;
        }

        public async UniTask AsyncInitialize()
        {
            _ballPositionable.Position =
                new Vector3
                (
                    _targetPositionable.Position.x + _initialOffset.x,
                    _targetPositionable.Position.y + _initialOffset.y,
                    0f
                );

            _stickyOffset = _initialOffset;
            
            await UniTask.CompletedTask;
        }

        public void Tick()
        {
            _ballPositionable.Position =
                new Vector3
                (
                    _targetPositionable.Position.x + _stickyOffset.x,
                    _ballPositionable.Position.y,
                    0f
                );
        }

        public void Restart()
        {
            _stickyOffset = _ballPositionable.Position - _targetPositionable.Position;
        }
    }
}